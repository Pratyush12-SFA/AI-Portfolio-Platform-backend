using AIPortfolio.Application.Abstractions;
using AIPortfolio.Domain.Entites.Portfolio;
using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Portfolio.Queries.GetSocialLinks;

internal sealed class GetSocialLinksQueryHandler(
    IPortfolioRepository repository,
    IUserInfoAccessor userInfoAccessor) : IRequestHandler<GetSocialLinksQuery, Result<IEnumerable<PortfolioSocialLink>>>
{
    public async Task<Result<IEnumerable<PortfolioSocialLink>>> Handle(GetSocialLinksQuery query,
        CancellationToken cancellationToken)
    {
        if (!userInfoAccessor.IsAuthenticated) return Result.Unauthorized();
        var data = await repository.GetSocialLinksByUserIdAsync(userInfoAccessor.UserId);
        return Result.Success(data);
    }
}