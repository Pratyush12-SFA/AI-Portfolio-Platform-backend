using AIPortfolio.Application.DTOs.Auth;
using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Auth.Queries.GetActiveSessions;

public sealed record GetActiveSessionsQuery(long UserId, string? CurrentRefreshToken)
    : IRequest<Result<IEnumerable<UserSessionResponse>>>;