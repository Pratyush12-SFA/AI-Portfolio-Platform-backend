using AIPortfolio.Application.Abstractions;
using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Portfolio.Commands.DeleteEducation;

internal sealed class DeleteEducationCommandHandler(
    IPortfolioRepository repository,
    IUserInfoAccessor userInfoAccessor) : IRequestHandler<DeleteEducationCommand, Result>
{
    public async Task<Result> Handle(DeleteEducationCommand command, CancellationToken cancellationToken)
    {
        if (!userInfoAccessor.IsAuthenticated) return Result.Unauthorized();
        await repository.DeleteEducationAsync(command.Id, userInfoAccessor.UserId);
        return Result.Success();
    }
}