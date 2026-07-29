using AIPortfolio.Application.Abstractions;
using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Portfolio.Commands.DeleteCustomSection;

internal sealed class DeleteCustomSectionCommandHandler(
    IPortfolioRepository repository,
    IUserInfoAccessor userInfoAccessor) : IRequestHandler<DeleteCustomSectionCommand, Result>
{
    public async Task<Result> Handle(DeleteCustomSectionCommand command, CancellationToken cancellationToken)
    {
        if (!userInfoAccessor.IsAuthenticated) return Result.Unauthorized();
        await repository.DeleteCustomSectionAsync(command.Id, userInfoAccessor.UserId);
        return Result.Success();
    }
}