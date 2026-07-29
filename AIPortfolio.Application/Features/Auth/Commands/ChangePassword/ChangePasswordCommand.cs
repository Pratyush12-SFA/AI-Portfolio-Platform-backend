using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Auth.Commands.ChangePassword;

public sealed record ChangePasswordCommand(long UserId, string OldPassword, string NewPassword) : IRequest<Result>;