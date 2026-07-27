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
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUserSessionRepository _userSessionRepository;

    public AuthService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator, IUserInfoAccessor userInfoAccessor,
        IRefreshTokenGenerator refreshTokenGenerator, IRefreshTokenRepository refreshTokenRepository,
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
        string refreshToken =
            _refreshTokenGenerator.Generate();
        long refreshTokenId =
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

                    IsActive = true,
                    CreatedBy = user.Email
                });
        return new LoginResponse
        {
            AccessToken = token,
            RefreshToken =  refreshToken,
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
            IsActive = true,
            CreatedBy = registerRequest.FullName
        };
        long id = await _userRepository.CreateAsync(user,
            _userInfoAccessor.GetUserName(),
            _userInfoAccessor.GetRemoteIp());

        string token = _jwtTokenGenerator.GenerateToken(
            id,
            user.Email,
            user.FullName);
        string refreshToken = _refreshTokenGenerator.Generate();    
        await _refreshTokenRepository.CreateAsync(
            new RefreshToken
            {
                UserId = id,
                Token = refreshToken,
                ExpiresAt = DateTime.Now.AddDays(30),
                CreatedBy = user.FullName,
                CreatedFromIp = _userInfoAccessor.GetRemoteIp()
            });
        return new LoginResponse
        {
            AccessToken = token,
            RefreshToken = refreshToken,
            Email = user.Email,
            FullName = user.FullName
        };

    }

    public async Task<bool> SendVerificationEmailAsync(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        if (user is null) return false;

        string token = Guid.NewGuid().ToString("N");
        DateTime expires = DateTime.UtcNow.AddHours(24);
        await _userRepository.UpdateSecurityTokensAsync(user.Id, user.ResetPasswordToken, user.ResetPasswordExpiresAt, token, expires);

        // Mock sending email - output to Console/Debug
        Console.WriteLine($"[EMAIL MOCK] Verification link: http://localhost:5173/verify-email?token={token}");
        return true;
    }

    public async Task<bool> VerifyEmailAsync(string token)
    {
        var user = await _userRepository.GetByVerificationTokenAsync(token);
        if (user is null) return false;

        await _userRepository.VerifyEmailAsync(user.Id);
        return true;
    }

    public async Task<bool> ForgotPasswordAsync(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        if (user is null) return false;

        string token = Guid.NewGuid().ToString("N");
        DateTime expires = DateTime.UtcNow.AddHours(1);
        await _userRepository.UpdateSecurityTokensAsync(user.Id, token, expires, user.VerificationToken, user.VerificationExpiresAt);

        // Mock sending email - output to Console/Debug
        Console.WriteLine($"[EMAIL MOCK] Password Reset link: http://localhost:5173/reset-password?token={token}");
        return true;
    }

    public async Task<bool> ResetPasswordAsync(string token, string newPassword)
    {
        var user = await _userRepository.GetByResetTokenAsync(token);
        if (user is null) return false;

        string hashed = _passwordHasher.Hash(newPassword);
        await _userRepository.UpdatePasswordAsync(user.Id, hashed);
        return true;
    }

    public async Task<bool> ChangePasswordAsync(long userId, string oldPassword, string newPassword)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user is null || string.IsNullOrEmpty(user.PasswordHash)) return false;

        if (!_passwordHasher.Verify(oldPassword, user.PasswordHash))
        {
            return false;
        }

        string hashed = _passwordHasher.Hash(newPassword);
        await _userRepository.UpdatePasswordAsync(userId, hashed);
        return true;
    }
}