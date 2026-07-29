namespace AIPortfolio.API.Middleware;

public static class ExceptionMiddlewareExtensions
{
    public static IApplicationBuilder
        UseGlobalExceptionHandling(this IApplicationBuilder applicationBuilder)
    {
        return applicationBuilder.UseMiddleware<ExceptionMiddleware>();
    }
}