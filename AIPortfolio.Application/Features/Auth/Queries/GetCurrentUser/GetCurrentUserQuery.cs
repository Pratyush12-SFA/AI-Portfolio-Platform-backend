using AIPortfolio.Application.DTOs.Auth;
using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Auth.Queries.GetCurrentUser;

public sealed record GetCurrentUserQuery : IRequest<Result<CurrentUserResponse>>;