using AIPortfolio.Application.Abstractions;
using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Auth.Commands.Logout;

internal sealed class LogoutCommandHandler : IRequestHandler<LogoutCommand, Result>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUserSessionRepository _userSessionRepository;

    public LogoutCommandHandler(
        IRefreshTokenRepository refreshTokenRepository,
        IUserSessionRepository userSessionRepository)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _userSessionRepository = userSessionRepository;
    }

    public async Task<Result> Handle(LogoutCommand command, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(command.RefreshToken)) return Result.Success();

        var refreshToken = await _refreshTokenRepository.GetByTokenAsync(command.RefreshToken);
        if (refreshToken is null) return Result.Success();

        await _userSessionRepository.RevokeSessionAsync(refreshToken.Id, "System");
        await _refreshTokenRepository.RemoveAsync(command.RefreshToken, "System");

        return Result.Success();
    }
}