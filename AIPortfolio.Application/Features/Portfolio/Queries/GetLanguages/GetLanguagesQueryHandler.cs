using AIPortfolio.Application.Abstractions;
using AIPortfolio.Domain.Entites.Resume;
using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Portfolio.Queries.GetLanguages;

internal sealed class GetLanguagesQueryHandler(
    IPortfolioRepository repository,
    IUserInfoAccessor userInfoAccessor) : IRequestHandler<GetLanguagesQuery, Result<IEnumerable<ResumeLanguage>>>
{
    public async Task<Result<IEnumerable<ResumeLanguage>>> Handle(GetLanguagesQuery query,
        CancellationToken cancellationToken)
    {
        if (!userInfoAccessor.IsAuthenticated) return Result.Unauthorized();
        var data = await repository.GetLanguagesByUserIdAsync(userInfoAccessor.UserId);
        return Result.Success(data);
    }
}