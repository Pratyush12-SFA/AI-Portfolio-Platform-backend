using System.Diagnostics;
using Ardalis.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AIPortfolio.Application.Behaviours;

public sealed class ElapsedTimeBehaviour<TCommand, TResponse>(ILogger<ElapsedTimeBehaviour<TCommand, TResponse>> logger)
    : IPipelineBehavior<TCommand, TResponse>
    where TCommand : IRequest<TResponse>
    where TResponse : class, IResult
{
    public async Task<TResponse> Handle(TCommand command, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var result = await next();
            stopwatch.Stop();
            logger.LogInformation("Elapsed time for {FullName}: {ElapsedMilliseconds} ms",
                typeof(TCommand).FullName, stopwatch.ElapsedMilliseconds);
            return result;
        }
        catch (Exception e)
        {
            if (stopwatch.IsRunning) stopwatch.Stop();
            logger.LogError(e, "Error while executing {FullName}. Elapsed: {ElapsedMilliseconds} ms",
                typeof(TCommand).FullName, stopwatch.ElapsedMilliseconds);
            throw;
        }
    }
}