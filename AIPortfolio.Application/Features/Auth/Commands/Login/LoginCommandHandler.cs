using AIPortfolio.Application.Abstractions;
using AIPortfolio.Application.DTOs.Auth;
using AIPortfolio.Domain.Entites;
using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Auth.Commands.Login;

internal sealed class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUserInfoAccessor _userInfoAccessor;
    private readonly IUserRepository _userRepository;
    private readonly IUserSessionRepository _userSessionRepository;

    public LoginCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator,
        IUserInfoAccessor userInfoAccessor,
        IRefreshTokenGenerator refreshTokenGenerator,
        IRefreshTokenRepository refreshTokenRepository,
        IUserSessionRepository userSessionRepository)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _userInfoAccessor = userInfoAccessor;
        _refreshTokenGenerator = refreshTokenGenerator;
        _refreshTokenRepository = refreshTokenRepository;
        _userSessionRepository = userSessionRepository;
    }

    public async Task<Result<LoginResponse>> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(command.Email);
        if (user is null) return Result.Unauthorized();

        if (string.IsNullOrWhiteSpace(user.PasswordHash)) return Result.Unauthorized();

        var isValid = _passwordHasher.Verify(command.Password, user.PasswordHash);
        if (!isValid) return Result.Unauthorized();

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
                IsActive = true,
                CreatedBy = user.Email
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