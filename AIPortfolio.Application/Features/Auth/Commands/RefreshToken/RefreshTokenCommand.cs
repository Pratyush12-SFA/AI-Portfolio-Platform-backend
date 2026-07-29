using AIPortfolio.Application.DTOs.Auth;
using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Auth.Commands.RefreshToken;

public sealed record RefreshTokenCommand(string RefreshToken) : IRequest<Result<RefreshTokenResponse>>;