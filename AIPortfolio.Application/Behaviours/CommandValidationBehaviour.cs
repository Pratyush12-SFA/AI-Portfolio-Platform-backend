using System.Reflection;
using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace AIPortfolio.Application.Behaviours;

public sealed class CommandValidationBehaviour<TCommand, TResponse>(IEnumerable<IValidator<TCommand>> validators)
    : IPipelineBehavior<TCommand, TResponse>
    where TCommand : IRequest<TResponse>
    where TResponse : class, IResult
{
    public async Task<TResponse> Handle(TCommand command, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!validators.Any()) return await next();

        ValidationContext<TCommand> context = new(command);

        IEnumerable<Task<ValidationResult>> validationTasks =
            validators.Select(v => v.ValidateAsync(context, cancellationToken));
        var results = await Task.WhenAll(validationTasks).ConfigureAwait(false);

        ValidationError[] errors =
        [
            .. results
                .Where(r => !r.IsValid)
                .SelectMany(r => r.AsErrors())
        ];

        if (errors.Length == 0) return await next();

        var response = typeof(TResponse)
                           .GetMethod(nameof(Result.Invalid), BindingFlags.Static | BindingFlags.Public,
                               [typeof(IEnumerable<ValidationError>)])?
                           .Invoke(null, [errors]) as TResponse
                       ?? Result.Invalid(errors) as TResponse;

        return response ?? throw new ValidationException("Validation errors occurred.");
    }
}