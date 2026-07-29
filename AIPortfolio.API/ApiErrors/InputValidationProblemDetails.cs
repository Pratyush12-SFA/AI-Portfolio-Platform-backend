using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;

namespace AIPortfolio.API.ApiErrors;

public sealed class InputValidationProblemDetails : ProblemDetails, IApiProblemDetails
{
    public InputValidationProblemDetails(IEnumerable<ValidationError> errors)
    {
        Title = "Validation errors.";
        Detail = "One or more validation errors occurred.";
        Errors = errors ?? [];
    }

    public IEnumerable<ValidationError> Errors { get; }

    public ErrorCode Code { get; } = ErrorCode.Validation;
}