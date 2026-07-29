using AIPortfolio.Application.DTOs.Auth;
using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Auth.Commands.Register;

public sealed record RegisterCommand(string FullName, string Email, string Password) : IRequest<Result<LoginResponse>>;