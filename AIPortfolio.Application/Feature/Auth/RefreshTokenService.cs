using AIPortfolio.Application.Abstractions;
using AIPortfolio.Application.DTOs.Auth;
using AIPortfolio.Domain.Entites;

namespace AIPortfolio.Application.Feature.Auth;

public sealed class RefreshTokenService : IRefreshTokenService
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUserInfoAccessor _userInfo;
    private readonly IUserRepository _userRepository;

    public RefreshTokenService(
        IRefreshTokenRepository refreshTokenRepository,
        IUserRepository userRepository,
        IJwtTokenGenerator jwtTokenGenerator,
        IRefreshTokenGenerator refreshTokenGenerator,
        IUserInfoAccessor userInfo
    )
    {
        _refreshTokenRepository =
            refreshTokenRepository ?? throw new ArgumentNullException(nameof(refreshTokenRepository));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _jwtTokenGenerator = jwtTokenGenerator ?? throw new ArgumentNullException(nameof(jwtTokenGenerator));
        _refreshTokenGenerator =
            refreshTokenGenerator ?? throw new ArgumentNullException(nameof(refreshTokenGenerator));
        _userInfo = userInfo ?? throw new ArgumentNullException(nameof(userInfo));
    }

    public async Task<RefreshTokenResponse?> RefreshAsync(RefreshTokenRequest refreshTokenRequest)
    {
        if (refreshTokenRequest.RefreshToken != null)
        {
            var refreshToken =
                await _refreshTokenRepository.GetByTokenAsync(
                    refreshTokenRequest.RefreshToken);
            if (refreshToken is null) return null;

            if (refreshToken.IsRevoked) return null;

            if (refreshToken.ExpiresAt <= DateTime.UtcNow) return null;
            var user =
                await _userRepository.GetByIdAsync(
                    refreshToken.UserId);

            if (user is null) return null;

            var accessToken =
                _jwtTokenGenerator.GenerateToken(
                    user.Id,
                    user.Email,
                    user.FullName);

            await _refreshTokenRepository.RemoveAsync(
                refreshToken.Token,
                user.FullName);

            var newRefreshToken =
                _refreshTokenGenerator.Generate();

            await _refreshTokenRepository.CreateAsync(
                new RefreshToken
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

        return null;
    }
}