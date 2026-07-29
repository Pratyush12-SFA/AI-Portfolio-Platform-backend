using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Auth.Commands.ResetPassword;

public sealed record ResetPasswordCommand(string Token, string NewPassword) : IRequest<Result>;