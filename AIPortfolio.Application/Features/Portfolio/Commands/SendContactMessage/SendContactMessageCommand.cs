using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Portfolio.Commands.SendContactMessage;

public sealed record SendContactMessageCommand(
    string Name,
    string Email,
    string Subject,
    string Message
) : IRequest<Result>;