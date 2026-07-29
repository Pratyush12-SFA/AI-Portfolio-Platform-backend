using AIPortfolio.API.Common;
using AIPortfolio.API.Extensions;

namespace AIPortfolio.API.Endpoints.Auth;

internal sealed class LoginEndpointMapper : IEndpointMapper
{
    public void Map(
        IEndpointRouteBuilder endpointRouteBuilder)
    {
        ArgumentNullException.ThrowIfNull(endpointRouteBuilder);

        var authGroup = endpointRouteBuilder.MapAuthGroup();

        authGroup.MapPost("login", LoginEndpoint.PostLogin)
            .WithDisplayName("login")
            .WithName("Login")
            .WithDescription("Login to portfolio application")
            .AllowAnonymous();

        authGroup.MapGet("me", LoginEndpoint.GetMe)
            .RequireAuthorization()
            .WithDisplayName("Current User")
            .WithName("CurrentUser");
    }
}