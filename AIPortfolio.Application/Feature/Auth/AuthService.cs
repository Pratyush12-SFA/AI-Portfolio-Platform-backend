using AIPortfolio.Application.Abstractions;
using AIPortfolio.Application.DTOs.Auth;
using AIPortfolio.Domain.Entites;

namespace AIPortfolio.Application.Feature.Auth;

public sealed class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserInfoAccessor _userInfoAccessor;

    public AuthService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator, IUserInfoAccessor userInfoAccessor)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _userInfoAccessor = userInfoAccessor;
    }

    public async Task<LoginResponse?> LoginAsync(
        LoginRequest request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user is null)
        {
            return null;
        }

        if (string.IsNullOrWhiteSpace(user.PasswordHash))
        {
            return null;
        }

        bool isValid = _passwordHasher.Verify(
            request.Password,
            user.PasswordHash);
        if (!isValid)
        {
            return null;
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

    public async Task<LoginResponse> RegisterAsync(RegisterRequest registerRequest)
    {
        bool exists = await _userRepository.ExistsByEmailAsync(
            registerRequest.Email);

        if (exists)
        {
            return null!;
        }

        string passwordHash = _passwordHasher.Hash(
            registerRequest.Password);

        User user = new()
        {
            FullName = registerRequest.FullName,
            PasswordHash = passwordHash,
            Email = registerRequest.Email,
            IsActive = true
        };
        long id = await _userRepository.CreateAsync(user,
            _userInfoAccessor.GetUserName(),
            _userInfoAccessor.GetRemoteIp());

        string token = _jwtTokenGenerator.GenerateToken(
            id,
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