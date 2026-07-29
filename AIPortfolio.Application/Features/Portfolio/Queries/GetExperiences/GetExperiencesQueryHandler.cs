using AIPortfolio.Application.Abstractions;
using AIPortfolio.Domain.Entites.Resume;
using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Portfolio.Queries.GetExperiences;

internal sealed class GetExperiencesQueryHandler(
    IPortfolioRepository repository,
    IUserInfoAccessor userInfoAccessor) : IRequestHandler<GetExperiencesQuery, Result<IEnumerable<ResumeExperience>>>
{
    public async Task<Result<IEnumerable<ResumeExperience>>> Handle(GetExperiencesQuery query,
        CancellationToken cancellationToken)
    {
        if (!userInfoAccessor.IsAuthenticated) return Result.Unauthorized();
        var data = await repository.GetExperiencesByUserIdAsync(userInfoAccessor.UserId);
        return Result.Success(data);
    }
}