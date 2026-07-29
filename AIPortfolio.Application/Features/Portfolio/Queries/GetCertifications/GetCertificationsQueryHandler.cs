using AIPortfolio.Application.Abstractions;
using AIPortfolio.Domain.Entites.Resume;
using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Portfolio.Queries.GetCertifications;

internal sealed class GetCertificationsQueryHandler(
    IPortfolioRepository repository,
    IUserInfoAccessor userInfoAccessor)
    : IRequestHandler<GetCertificationsQuery, Result<IEnumerable<ResumeCertification>>>
{
    public async Task<Result<IEnumerable<ResumeCertification>>> Handle(GetCertificationsQuery query,
        CancellationToken cancellationToken)
    {
        if (!userInfoAccessor.IsAuthenticated) return Result.Unauthorized();
        var data = await repository.GetCertificationsByUserIdAsync(userInfoAccessor.UserId);
        return Result.Success(data);
    }
}