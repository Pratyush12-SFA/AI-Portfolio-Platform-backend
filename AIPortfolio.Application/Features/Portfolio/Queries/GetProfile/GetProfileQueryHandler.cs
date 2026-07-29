using AIPortfolio.Application.Abstractions;
using Ardalis.Result;
using MediatR;
using PortfolioEntity = AIPortfolio.Domain.Entites.Portfolio.Portfolio;

namespace AIPortfolio.Application.Features.Portfolio.Queries.GetProfile;

internal sealed class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, Result<PortfolioEntity>>
{
    private readonly IPortfolioRepository _repository;
    private readonly IUserInfoAccessor _userInfoAccessor;

    public GetProfileQueryHandler(IPortfolioRepository repository, IUserInfoAccessor userInfoAccessor)
    {
        _repository = repository;
        _userInfoAccessor = userInfoAccessor;
    }

    public async Task<Result<PortfolioEntity>> Handle(GetProfileQuery query, CancellationToken cancellationToken)
    {
        if (!_userInfoAccessor.IsAuthenticated) return Result.Unauthorized();

        var profile = await _repository.GetProfileByUserIdAsync(_userInfoAccessor.UserId);
        if (profile is null)
            profile = new PortfolioEntity
            {
                UserId = _userInfoAccessor.UserId,
                CustomSlug = "user-" + _userInfoAccessor.UserId,
                IsPublic = true,
                CreatedBy = _userInfoAccessor.Email ?? "system",
                CreatedFromIp = _userInfoAccessor.GetRemoteIp() ?? "127.0.0.1",
                RowVersion = new byte[8]
            };
        return profile;
    }
}