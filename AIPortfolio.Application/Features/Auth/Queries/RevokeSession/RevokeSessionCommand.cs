using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Auth.Queries.RevokeSession;

public sealed record RevokeSessionCommand(long RefreshTokenId, string? RevokedBy) : IRequest<Result>;