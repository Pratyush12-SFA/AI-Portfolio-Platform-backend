namespace AIPortfolio.API.ApiErrors;

public interface IApiProblemDetails
{
    public ErrorCode Code { get; }
}