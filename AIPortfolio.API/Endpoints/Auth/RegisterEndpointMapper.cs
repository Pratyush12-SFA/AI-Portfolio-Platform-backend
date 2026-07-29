using AIPortfolio.API.Common;
using AIPortfolio.API.Extensions;

namespace AIPortfolio.API.Endpoints.Auth;

internal sealed class RegisterEndpointMapper : IEndpointMapper
{
    public void Map(
        IEndpointRouteBuilder endpointRouteBuilder)
    {
        ArgumentNullException.ThrowIfNull(endpointRouteBuilder);

        var authGroup = endpointRouteBuilder.MapAuthGroup();
        authGroup.MapPost(
                "/register",
                RegisterEndpoint.PostRegister)
            .AllowAnonymous()
            .WithName("Register")
            .WithDescription("Register a new user.");
    }
}