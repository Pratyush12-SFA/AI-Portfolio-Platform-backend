using AIPortfolio.Application.Abstractions;
using AIPortfolio.Domain.Entites.Resume;
using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Portfolio.Queries.GetCustomSections;

internal sealed class GetCustomSectionsQueryHandler(
    IPortfolioRepository repository,
    IUserInfoAccessor userInfoAccessor)
    : IRequestHandler<GetCustomSectionsQuery, Result<IEnumerable<ResumeCustomSection>>>
{
    public async Task<Result<IEnumerable<ResumeCustomSection>>> Handle(GetCustomSectionsQuery query,
        CancellationToken cancellationToken)
    {
        if (!userInfoAccessor.IsAuthenticated) return Result.Unauthorized();
        var data = await repository.GetCustomSectionsByUserIdAsync(userInfoAccessor.UserId);
        return Result.Success(data);
    }
}