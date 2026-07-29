using AIPortfolio.Application.Abstractions;
using AIPortfolio.Domain.Entites.Resume;
using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Portfolio.Queries.GetAchievements;

internal sealed class GetAchievementsQueryHandler(
    IPortfolioRepository repository,
    IUserInfoAccessor userInfoAccessor) : IRequestHandler<GetAchievementsQuery, Result<IEnumerable<ResumeAchievement>>>
{
    public async Task<Result<IEnumerable<ResumeAchievement>>> Handle(GetAchievementsQuery query,
        CancellationToken cancellationToken)
    {
        if (!userInfoAccessor.IsAuthenticated) return Result.Unauthorized();
        var data = await repository.GetAchievementsByUserIdAsync(userInfoAccessor.UserId);
        return Result.Success(data);
    }
}