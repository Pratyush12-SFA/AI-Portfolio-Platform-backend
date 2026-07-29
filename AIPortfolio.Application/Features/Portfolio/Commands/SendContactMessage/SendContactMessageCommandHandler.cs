using Ardalis.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AIPortfolio.Application.Features.Portfolio.Commands.SendContactMessage;

internal sealed class SendContactMessageCommandHandler(
    ILogger<SendContactMessageCommandHandler> logger) : IRequestHandler<SendContactMessageCommand, Result>
{
    public Task<Result> Handle(SendContactMessageCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "[CONTACT MESSAGE] From: {Name} ({Email}) - Subject: {Subject} - Message: {Message}",
            command.Name, command.Email, command.Subject, command.Message);
        return Task.FromResult(Result.Success());
    }
}