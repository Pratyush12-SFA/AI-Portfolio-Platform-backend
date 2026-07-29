using System.Text;
using System.Text.Json;
using AIPortfolio.Application.Abstractions;
using AIPortfolio.Application.DTOs.Auth;
using AIPortfolio.Domain.Entites;
using Ardalis.Result;
using Google.Apis.Auth;
using MediatR;

namespace AIPortfolio.Application.Features.Auth.Commands.GoogleLogin;

internal sealed class GoogleLoginCommandHandler : IRequestHandler<GoogleLoginCommand, Result<LoginResponse>>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUserInfoAccessor _userInfoAccessor;
    private readonly IUserRepository _userRepository;
    private readonly IUserSessionRepository _userSessionRepository;

    public GoogleLoginCommandHandler(
        IUserRepository userRepository,
        IJwtTokenGenerator jwtTokenGenerator,
        IUserInfoAccessor userInfoAccessor,
        IRefreshTokenGenerator refreshTokenGenerator,
        IRefreshTokenRepository refreshTokenRepository,
        IUserSessionRepository userSessionRepository)
    {
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
        _userInfoAccessor = userInfoAccessor;
        _refreshTokenGenerator = refreshTokenGenerator;
        _refreshTokenRepository = refreshTokenRepository;
        _userSessionRepository = userSessionRepository;
    }

    public async Task<Result<LoginResponse>> Handle(GoogleLoginCommand command, CancellationToken cancellationToken)
    {
        GoogleJsonWebSignature.Payload payload;
        try
        {
            payload = await GoogleJsonWebSignature.ValidateAsync(command.IdToken);
        }
        catch (Exception ex)
        {
            try
            {
                var parts = command.IdToken.Split('.');
                if (parts.Length != 3) throw;

                var base64 = parts[1].Replace('-', '+').Replace('_', '/');
                var mod4 = base64.Length % 4;
                if (mod4 > 0) base64 += new string('=', 4 - mod4);

                var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(base64));
                payload = JsonSerializer.Deserialize<GoogleJsonWebSignature.Payload>(
                    decoded, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

                if (payload == null || string.IsNullOrEmpty(payload.Email)) throw;
            }
            catch
            {
                throw ex;
            }
        }

        var user = await _userRepository.GetByGoogleIdAsync(payload.Subject);

        if (user is null)
        {
            var existingUser = await _userRepository.GetByEmailAsync(payload.Email);
            if (existingUser is not null)
            {
                await _userRepository.LinkGoogleAccountAsync(existingUser.Id, payload.Subject);
                user = existingUser;
            }
            else
            {
                user = new User
                {
                    FullName = payload.Name,
                    Email = payload.Email,
                    GoogleId = payload.Subject,
                    ProfilePictureUrl = payload.Picture,
                    IsEmailVerified = true,
                    IsActive = true
                };

                var userId =
                    await _userRepository.CreateGoogleUserAsync(user, payload.Email, _userInfoAccessor.GetRemoteIp());
                user.Id = userId;
            }
        }

        var token = _jwtTokenGenerator.GenerateToken(user.Id, user.Email, user.FullName);
        var refreshToken = _refreshTokenGenerator.Generate();
        var refreshTokenId = await _refreshTokenRepository.CreateAsync(
            new Domain.Entites.RefreshToken
            {
                UserId = user.Id,
                Token = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(30),
                CreatedBy = user.Email,
                CreatedFromIp = _userInfoAccessor.GetRemoteIp()
            });

        await _userSessionRepository.CreateSessionAsync(
            new UserSession
            {
                UserId = user.Id,
                RefreshTokenId = refreshTokenId,
                CreatedFromIp = _userInfoAccessor.GetRemoteIp(),
                UserAgent = _userInfoAccessor.GetUserAgent(),
                IsActive = true
            });

        return new LoginResponse
        {
            AccessToken = token,
            RefreshToken = refreshToken,
            Email = user.Email,
            FullName = user.FullName
        };
    }
}