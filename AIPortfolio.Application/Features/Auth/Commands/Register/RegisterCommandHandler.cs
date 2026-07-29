using AIPortfolio.Application.Abstractions;
using AIPortfolio.Application.DTOs.Auth;
using AIPortfolio.Domain.Entites;
using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Auth.Commands.Register;

internal sealed class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<LoginResponse>>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUserInfoAccessor _userInfoAccessor;
    private readonly IUserRepository _userRepository;

    public RegisterCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator,
        IUserInfoAccessor userInfoAccessor,
        IRefreshTokenGenerator refreshTokenGenerator,
        IRefreshTokenRepository refreshTokenRepository)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _userInfoAccessor = userInfoAccessor;
        _refreshTokenGenerator = refreshTokenGenerator;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<Result<LoginResponse>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        var exists = await _userRepository.ExistsByEmailAsync(command.Email);
        if (exists) return Result.Conflict("User with this email already exists.");

        var passwordHash = _passwordHasher.Hash(command.Password);

        User user = new()
        {
            FullName = command.FullName,
            PasswordHash = passwordHash,
            Email = command.Email,
            IsActive = true,
            CreatedBy = command.FullName
        };

        var id = await _userRepository.CreateAsync(user, _userInfoAccessor.GetUserName(),
            _userInfoAccessor.GetRemoteIp());

        var token = _jwtTokenGenerator.GenerateToken(id, user.Email, user.FullName);
        var refreshToken = _refreshTokenGenerator.Generate();
        await _refreshTokenRepository.CreateAsync(
            new Domain.Entites.RefreshToken
            {
                UserId = id,
                Token = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(30),
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
}