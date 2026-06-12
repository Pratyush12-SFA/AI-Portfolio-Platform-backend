using AIPortfolio.Application.Abstractions;
using AIPortfolio.Application.DTOs.Auth;
using AIPortfolio.Domain.Entites;
using Google.Apis.Auth;

namespace AIPortfolio.Application.Feature.Auth;

public sealed class GoogleAuthService : IGoogleAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserInfoAccessor _userInfoAccessor;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public GoogleAuthService(
        IUserRepository userRepository,
        IJwtTokenGenerator jwtTokenGenerator,
        IUserInfoAccessor userInfoAccessor,
        IRefreshTokenGenerator refreshTokenGenerator,
        IRefreshTokenRepository refreshTokenRepository
        )
    {
        _userRepository =  userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _jwtTokenGenerator = jwtTokenGenerator ?? throw new ArgumentNullException(nameof(jwtTokenGenerator));
        _userInfoAccessor = userInfoAccessor  ?? throw new ArgumentNullException(nameof(userInfoAccessor));
        _refreshTokenRepository = refreshTokenRepository
                                  ?? throw new ArgumentNullException(nameof(refreshTokenRepository));
        _refreshTokenGenerator = refreshTokenGenerator ?? throw new ArgumentNullException(nameof(refreshTokenGenerator));
    }
    
    public async Task<LoginResponse?>LoginAsync(
        GoogleLoginRequest request)
    {
        GoogleJsonWebSignature.Payload payload =
            await GoogleJsonWebSignature.ValidateAsync(
                request.IdToken);

        User? user =
            await _userRepository.GetByGoogleIdAsync(
                payload.Subject);

        if (user is null)
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

            long userId =
                await _userRepository.CreateGoogleUserAsync(
                    user,
                    payload.Email,
                    _userInfoAccessor.GetRemoteIp());

            user.Id = userId;
        }

        string token =
            _jwtTokenGenerator.GenerateToken(
                user.Id,
                user.Email,
                user.FullName);
        string refreshToken =
            _refreshTokenGenerator.Generate();
        await _refreshTokenRepository.CreateAsync(
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

        return new LoginResponse
        {
            AccessToken = token,
            RefreshToken = refreshToken,
            Email = user.Email,
            FullName = user.FullName
        };
    }
    
}