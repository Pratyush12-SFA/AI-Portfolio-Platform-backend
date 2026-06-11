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

    public GoogleAuthService(
        IUserRepository userRepository,
        IJwtTokenGenerator jwtTokenGenerator,
        IUserInfoAccessor userInfoAccessor)
    {
        _userRepository =  userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _jwtTokenGenerator = jwtTokenGenerator ?? throw new ArgumentNullException(nameof(jwtTokenGenerator));
        _userInfoAccessor = userInfoAccessor  ?? throw new ArgumentNullException(nameof(userInfoAccessor));
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

        return new LoginResponse
        {
            AccessToken = token,
            Email = user.Email,
            FullName = user.FullName
        };
    }
    
}