using AIPortfolio.Application.Abstractions;
using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Portfolio.Commands.DeleteAchievement;

internal sealed class DeleteAchievementCommandHandler(
    IPortfolioRepository repository,
    IUserInfoAccessor userInfoAccessor) : IRequestHandler<DeleteAchievementCommand, Result>
{
    public async Task<Result> Handle(DeleteAchievementCommand command, CancellationToken cancellationToken)
    {
        if (!userInfoAccessor.IsAuthenticated) return Result.Unauthorized();
        await repository.DeleteAchievementAsync(command.Id, userInfoAccessor.UserId);
        return Result.Success();
    }
}