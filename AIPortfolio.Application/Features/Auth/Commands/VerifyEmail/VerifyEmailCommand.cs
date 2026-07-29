using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Auth.Commands.VerifyEmail;

public sealed record VerifyEmailCommand(string Token) : IRequest<Result>;