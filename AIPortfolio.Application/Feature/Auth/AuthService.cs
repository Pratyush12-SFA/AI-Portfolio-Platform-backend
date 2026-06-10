using AIPortfolio.Application.Abstractions;
using AIPortfolio.Application.DTOs.Auth;

namespace AIPortfolio.Application.Feature.Auth;

public sealed class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<LoginResponse?> LoginAsync(
        LoginRequest request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user is null)
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
}