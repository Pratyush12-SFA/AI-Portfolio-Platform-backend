using AIPortfolio.Application.Abstractions;
using AIPortfolio.Domain.Entites.Resume;
using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Portfolio.Queries.GetSkills;

internal sealed class GetSkillsQueryHandler(
    IPortfolioRepository repository,
    IUserInfoAccessor userInfoAccessor) : IRequestHandler<GetSkillsQuery, Result<IEnumerable<ResumeSkill>>>
{
    public async Task<Result<IEnumerable<ResumeSkill>>> Handle(GetSkillsQuery query,
        CancellationToken cancellationToken)
    {
        if (!userInfoAccessor.IsAuthenticated) return Result.Unauthorized();
        var data = await repository.GetSkillsByUserIdAsync(userInfoAccessor.UserId);
        return Result.Success(data);
    }
}