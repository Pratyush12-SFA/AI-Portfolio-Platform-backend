namespace AIPortfolio.API.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static RouteGroupBuilder MapAuthGroup(
        this IEndpointRouteBuilder endpointRouteBuilder
    )
    {
        return endpointRouteBuilder
            .MapGroup("/api/auth")
            .WithTags("Authentication");
    }
}