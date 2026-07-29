using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Auth.Commands.Logout;

public sealed record LogoutCommand(string? RefreshToken) : IRequest<Result>;