using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Auth.Commands.ForgotPassword;

public sealed record ForgotPasswordCommand(string Email) : IRequest<Result>;