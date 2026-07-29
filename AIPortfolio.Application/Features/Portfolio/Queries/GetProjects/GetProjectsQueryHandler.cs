using AIPortfolio.Application.Abstractions;
using AIPortfolio.Domain.Entites.Resume;
using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Portfolio.Queries.GetProjects;

internal sealed class GetProjectsQueryHandler(
    IPortfolioRepository repository,
    IUserInfoAccessor userInfoAccessor) : IRequestHandler<GetProjectsQuery, Result<IEnumerable<ResumeProject>>>
{
    public async Task<Result<IEnumerable<ResumeProject>>> Handle(GetProjectsQuery query,
        CancellationToken cancellationToken)
    {
        if (!userInfoAccessor.IsAuthenticated) return Result.Unauthorized();
        var data = await repository.GetProjectsByUserIdAsync(userInfoAccessor.UserId);
        return Result.Success(data);
    }
}