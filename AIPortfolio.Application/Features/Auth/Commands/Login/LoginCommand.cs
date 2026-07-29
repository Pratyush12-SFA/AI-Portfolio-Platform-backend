using AIPortfolio.Application.DTOs.Auth;
using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Auth.Commands.Login;

public sealed record LoginCommand(string Email, string Password, bool RememberMe) : IRequest<Result<LoginResponse>>;