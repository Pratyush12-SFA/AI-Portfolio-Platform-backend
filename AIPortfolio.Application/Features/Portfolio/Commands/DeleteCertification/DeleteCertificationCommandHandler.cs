using AIPortfolio.Application.Abstractions;
using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Portfolio.Commands.DeleteCertification;

internal sealed class DeleteCertificationCommandHandler(
    IPortfolioRepository repository,
    IUserInfoAccessor userInfoAccessor) : IRequestHandler<DeleteCertificationCommand, Result>
{
    public async Task<Result> Handle(DeleteCertificationCommand command, CancellationToken cancellationToken)
    {
        if (!userInfoAccessor.IsAuthenticated) return Result.Unauthorized();
        await repository.DeleteCertificationAsync(command.Id, userInfoAccessor.UserId);
        return Result.Success();
    }
}