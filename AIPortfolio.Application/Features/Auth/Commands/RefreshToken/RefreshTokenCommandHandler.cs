using AIPortfolio.Application.Abstractions;
using AIPortfolio.Application.DTOs.Auth;
using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Auth.Commands.RefreshToken;

internal sealed class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<RefreshTokenResponse>>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUserInfoAccessor _userInfo;
    private readonly IUserRepository _userRepository;

    public RefreshTokenCommandHandler(
        IRefreshTokenRepository refreshTokenRepository,
        IUserRepository userRepository,
        IJwtTokenGenerator jwtTokenGenerator,
        IRefreshTokenGenerator refreshTokenGenerator,
        IUserInfoAccessor userInfo)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
        _refreshTokenGenerator = refreshTokenGenerator;
        _userInfo = userInfo;
    }

    public async Task<Result<RefreshTokenResponse>> Handle(RefreshTokenCommand command,
        CancellationToken cancellationToken)
    {
        var refreshToken = await _refreshTokenRepository.GetByTokenAsync(command.RefreshToken);
        if (refreshToken is null) return Result.Unauthorized();

        if (refreshToken.IsRevoked) return Result.Unauthorized();

        if (refreshToken.ExpiresAt <= DateTime.UtcNow) return Result.Unauthorized();

        var user = await _userRepository.GetByIdAsync(refreshToken.UserId);
        if (user is null) return Result.Unauthorized();

        var accessToken = _jwtTokenGenerator.GenerateToken(user.Id, user.Email, user.FullName);

        await _refreshTokenRepository.RemoveAsync(refreshToken.Token, user.FullName);

        var newRefreshToken = _refreshTokenGenerator.Generate();
        await _refreshTokenRepository.CreateAsync(
            new Domain.Entites.RefreshToken
            {
                UserId = user.Id,
                Token = newRefreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(30),
                CreatedBy = user.FullName,
                CreatedFromIp = _userInfo.GetRemoteIp()
            });

        return new RefreshTokenResponse
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken
        };
    }
}