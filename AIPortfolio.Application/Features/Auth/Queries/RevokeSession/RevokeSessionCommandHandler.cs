using AIPortfolio.Application.Abstractions;
using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Auth.Queries.RevokeSession;

internal sealed class RevokeSessionCommandHandler : IRequestHandler<RevokeSessionCommand, Result>
{
    private readonly IUserSessionRepository _sessionRepository;

    public RevokeSessionCommandHandler(IUserSessionRepository sessionRepository)
    {
        _sessionRepository = sessionRepository;
    }

    public async Task<Result> Handle(RevokeSessionCommand command, CancellationToken cancellationToken)
    {
        await _sessionRepository.RevokeSessionAsync(command.RefreshTokenId, command.RevokedBy ?? "System");
        return Result.Success();
    }
}