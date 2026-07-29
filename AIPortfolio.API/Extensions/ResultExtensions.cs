using System.Net;
using AIPortfolio.API.ApiErrors;
using Ardalis.Result;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace AIPortfolio.API.Extensions;

public static class ResultExtensions
{
    public static async Task<IResult> ToApiResultAsync<T>(this Task<Result<T>> resultTask)
    {
        var result = await resultTask.ConfigureAwait(false);
        return result.ToApiResult();
    }

    public static async ValueTask<IResult> ToApiResultAsync<T>(this ValueTask<Result<T>> resultTask)
    {
        var result = await resultTask.ConfigureAwait(false);
        return result.ToApiResult();
    }

    public static async ValueTask<IResult> ToApiResultAsync(this ValueTask<Result> resultTask)
    {
        var result = await resultTask.ConfigureAwait(false);
        return result.ToApiResult();
    }

    public static async Task<IResult> ToApiResultAsync(this Task<Result> resultTask)
    {
        var result = await resultTask.ConfigureAwait(false);
        return result.ToApiResult();
    }

    public static IResult ToApiResult<T>(this Result<T> result)
    {
        return result.Status switch
        {
            ResultStatus.Ok => TypedResults.Ok(result.Value),
            ResultStatus.Invalid => ValidationProblem(result),
            ResultStatus.Conflict => Conflict(result),
            ResultStatus.NotFound => NotFound(result),
            ResultStatus.CriticalError => InternalServerError(result),
            ResultStatus.Error => InternalServerError(result),
            ResultStatus.NoContent => TypedResults.NoContent(),
            ResultStatus.Unauthorized => Unauthorized(result),
            ResultStatus.Forbidden => Forbidden(),
            _ => TypedResults.Empty
        };
    }

    public static IResult ToApiResult(this Result result)
    {
        return result.Status switch
        {
            ResultStatus.Ok => TypedResults.Ok(),
            ResultStatus.Invalid => ValidationProblem(result),
            ResultStatus.Conflict => Conflict(result),
            ResultStatus.NotFound => NotFound(result),
            ResultStatus.CriticalError => InternalServerError(result),
            ResultStatus.Error => InternalServerError(result),
            ResultStatus.NoContent => TypedResults.NoContent(),
            ResultStatus.Unauthorized => Unauthorized(result),
            ResultStatus.Forbidden => Forbidden(),
            _ => TypedResults.Empty
        };
    }

    private static IResult NotFound<T>(Result<T> result)
    {
        var problem = new ApiProblemDetails(ErrorCode.NotFound, HttpStatusCode.NotFound, "Not Found",
            result.Errors.FirstOrDefault());
        return TypedResults.NotFound(problem);
    }

    private static IResult NotFound(Result result)
    {
        var problem = new ApiProblemDetails(ErrorCode.NotFound, HttpStatusCode.NotFound, "Not Found",
            result.Errors.FirstOrDefault());
        return TypedResults.NotFound(problem);
    }

    private static IResult ValidationProblem<T>(Result<T> result)
    {
        var problem = new InputValidationProblemDetails(result.ValidationErrors);
        return TypedResults.BadRequest(problem);
    }

    private static IResult ValidationProblem(Result result)
    {
        var problem = new InputValidationProblemDetails(result.ValidationErrors);
        return TypedResults.BadRequest(problem);
    }

    private static IResult Conflict<T>(Result<T> result)
    {
        var problem = new ApiProblemDetails(ErrorCode.Conflict, HttpStatusCode.Conflict, "Conflict",
            result.Errors.FirstOrDefault());
        return TypedResults.Conflict(problem);
    }

    private static IResult Conflict(Result result)
    {
        var problem = new ApiProblemDetails(ErrorCode.Conflict, HttpStatusCode.Conflict, "Conflict",
            result.Errors.FirstOrDefault());
        return TypedResults.Conflict(problem);
    }

    private static IResult InternalServerError<T>(Result<T> result)
    {
        var problem = new ApiProblemDetails(ErrorCode.ServerError, HttpStatusCode.InternalServerError, "Server Error",
            result.Errors.FirstOrDefault());
        return TypedResults.InternalServerError(problem);
    }

    private static IResult InternalServerError(Result result)
    {
        var problem = new ApiProblemDetails(ErrorCode.ServerError, HttpStatusCode.InternalServerError, "Server Error",
            result.Errors.FirstOrDefault());
        return TypedResults.InternalServerError(problem);
    }

    private static IResult Unauthorized<T>(Result<T> result)
    {
        var problem = new ApiProblemDetails(ErrorCode.Unauthorized, HttpStatusCode.Unauthorized, "Unauthorized",
            result.Errors.FirstOrDefault());
        return TypedResults.Problem(problem);
    }

    private static IResult Forbidden()
    {
        var problem = new ApiProblemDetails(ErrorCode.Forbidden, HttpStatusCode.Forbidden, "Forbidden");
        return TypedResults.Problem(problem);
    }
}