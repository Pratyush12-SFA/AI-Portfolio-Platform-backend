using AIPortfolio.API.Common;
using AIPortfolio.API.Extensions;

namespace AIPortfolio.API.Endpoints.Auth;

internal sealed class LogoutEndpointMapper : IEndpointMapper
{
    public void Map(
        IEndpointRouteBuilder endpointRouteBuilder)
    {
        ArgumentNullException.ThrowIfNull(endpointRouteBuilder);

        var authGroup = endpointRouteBuilder.MapAuthGroup();

        authGroup.MapPost("logout", LogoutEndpoint.PostLogout)
            .WithDisplayName("logout")
            .WithName("Logout")
            .WithDescription("Logout to portfolio application")
            .AllowAnonymous();
    }
}