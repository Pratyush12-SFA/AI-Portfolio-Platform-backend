using AIPortfolio.Application.Abstractions;
using AIPortfolio.Domain.Entites.Resume;
using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Portfolio.Queries.GetEducations;

internal sealed class GetEducationsQueryHandler(
    IPortfolioRepository repository,
    IUserInfoAccessor userInfoAccessor) : IRequestHandler<GetEducationsQuery, Result<IEnumerable<ResumeEducation>>>
{
    public async Task<Result<IEnumerable<ResumeEducation>>> Handle(GetEducationsQuery query,
        CancellationToken cancellationToken)
    {
        if (!userInfoAccessor.IsAuthenticated) return Result.Unauthorized();
        var data = await repository.GetEducationsByUserIdAsync(userInfoAccessor.UserId);
        return Result.Success(data);
    }
}