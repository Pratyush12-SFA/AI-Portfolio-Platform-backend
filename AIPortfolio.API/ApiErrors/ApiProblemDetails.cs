using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace AIPortfolio.API.ApiErrors;

public sealed class ApiProblemDetails : ProblemDetails, IApiProblemDetails
{
    public ApiProblemDetails(ErrorCode errorCode, HttpStatusCode status, string title, string? detail = null)
    {
        Code = errorCode ?? throw new ArgumentNullException(nameof(errorCode));
        Status = (int)status;
        Title = title;
        Detail = detail;
    }

    public ErrorCode Code { get; }
}