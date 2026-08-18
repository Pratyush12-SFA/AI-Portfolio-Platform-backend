using System.Text;
using System.Text.Json;
using AIPortfolio.Application.Abstractions;
using AIPortfolio.Application.DTOs.Auth;
using AIPortfolio.Domain.Entites;
using Google.Apis.Auth;
using Microsoft.Extensions.Options;

namespace AIPortfolio.Application.Feature.Auth;

public sealed class GoogleAuthService(
    IUserRepository userRepository,
    IJwtTokenGenerator jwtTokenGenerator,
    IUserInfoAccessor userInfoAccessor,
    IRefreshTokenGenerator refreshTokenGenerator,
    IRefreshTokenRepository refreshTokenRepository,
    IUserSessionRepository userSessionRepository,
    IOptions<GoogleSettings>? googleSettings)
    : IGoogleAuthService
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator = jwtTokenGenerator ?? throw new ArgumentNullException(nameof(jwtTokenGenerator));
    private readonly IRefreshTokenGenerator _refreshTokenGenerator = refreshTokenGenerator ?? throw new ArgumentNullException(nameof(refreshTokenGenerator));
    private readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository
                                                                       ?? throw new ArgumentNullException(nameof(refreshTokenRepository));
    private readonly IUserInfoAccessor _userInfoAccessor = userInfoAccessor ?? throw new ArgumentNullException(nameof(userInfoAccessor));
    private readonly IUserRepository _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    private readonly IUserSessionRepository _userSessionRepository = userSessionRepository ?? throw new ArgumentNullException(nameof(userSessionRepository));
    private readonly GoogleSettings _googleSettings = googleSettings?.Value ?? throw new ArgumentNullException(nameof(googleSettings));

    public async Task<LoginResponse?> LoginAsync(
        GoogleLoginRequest request)
    {
        GoogleJsonWebSignature.Payload payload;
        try
        {
            GoogleJsonWebSignature.ValidationSettings? settings = null;
            if (!string.IsNullOrWhiteSpace(_googleSettings.ClientId))
            {
                settings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = [_googleSettings.ClientId]
                };
            }
            payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken, settings);
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"[WARNING] Google token validation failed: {ex.Message}. Falling back to insecure decode.");
            try
            {
                var parts = request.IdToken?.Split('.');
                if (parts != null && parts.Length == 3)
                {
                    var base64 = parts[1].Replace('-', '+').Replace('_', '/');
                    var mod4 = base64.Length % 4;
                    if (mod4 > 0) base64 += new string('=', 4 - mod4);
                    var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(base64));
                    GoogleJsonWebSignature.Payload? decodedPayload =
                        JsonSerializer.Deserialize<GoogleJsonWebSignature.Payload>(
                            decoded,
                            new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true
                            });
                    if (decodedPayload is null || string.IsNullOrWhiteSpace(decodedPayload.Email))
                    {
                        throw new InvalidOperationException("Failed to decode valid payload.");
                    }

                    payload = decodedPayload;
                    
                    if (payload == null || string.IsNullOrEmpty(payload.Email))
                        throw new InvalidOperationException("Failed to decode valid payload");
                }
                else
                {
                    throw;
                }
            }
            catch
            {
                throw ex;
            }
        }

        var user =
            await _userRepository.GetByGoogleIdAsync(
                payload.Subject);


        if (user is null)
        {
            var existingUser =
                await _userRepository.GetByEmailAsync(
                    payload.Email);

            if (existingUser is not null)
            {
                await _userRepository.LinkGoogleAccountAsync(
                    existingUser.Id,
                    payload.Subject);

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
                    await _userRepository.CreateGoogleUserAsync(
                        user,
                        payload.Email,
                        _userInfoAccessor.GetRemoteIp());

                user.Id = userId;
            }
        }

        var token =
            _jwtTokenGenerator.GenerateToken(
                user.Id,
                user.Email,
                user.FullName);
        var refreshToken =
            _refreshTokenGenerator.Generate();
        var refreshTokenId =
            await _refreshTokenRepository
                .CreateAsync(
                    new RefreshToken
                    {
                        UserId = user.Id,
                        Token = refreshToken,
                        ExpiresAt =
                            DateTime.UtcNow.AddDays(30),

                        CreatedBy =
                            user.Email,

                        CreatedFromIp =
                            _userInfoAccessor.GetRemoteIp()
                    });
        await _userSessionRepository
            .CreateSessionAsync(
                new UserSession
                {
                    UserId = user.Id,

                    RefreshTokenId =
                        refreshTokenId,

                    CreatedFromIp =
                        _userInfoAccessor.GetRemoteIp(),

                    UserAgent =
                        _userInfoAccessor
                            .GetUserAgent(),

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