namespace AIPortfolio.API.Middleware;

public sealed class ExceptionMiddleware
{
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly RequestDelegate _requestDelegate;

    public ExceptionMiddleware(
        RequestDelegate requestDelegate,
        ILogger<ExceptionMiddleware> logger)
    {
        _requestDelegate = requestDelegate ?? throw new ArgumentNullException(nameof(requestDelegate));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _requestDelegate(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "An exception occurred while processing your request.");
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(
                new
                {
                    Success = false, ex.Message
                });
        }
    }
}