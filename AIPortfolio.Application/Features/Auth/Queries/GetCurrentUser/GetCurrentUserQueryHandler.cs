using AIPortfolio.Application.Abstractions;
using AIPortfolio.Application.DTOs.Auth;
using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Auth.Queries.GetCurrentUser;

internal sealed class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, Result<CurrentUserResponse>>
{
    private readonly IUserInfoAccessor _userInfoAccessor;

    public GetCurrentUserQueryHandler(IUserInfoAccessor userInfoAccessor)
    {
        _userInfoAccessor = userInfoAccessor;
    }

    public Task<Result<CurrentUserResponse>> Handle(GetCurrentUserQuery query, CancellationToken cancellationToken)
    {
        var response = new CurrentUserResponse
        {
            UserId = _userInfoAccessor.UserId,
            Email = _userInfoAccessor.Email,
            FullName = _userInfoAccessor.FullName,
            IsAuthenticated = _userInfoAccessor.IsAuthenticated
        };

        return Task.FromResult(Result.Success(response));
    }
}