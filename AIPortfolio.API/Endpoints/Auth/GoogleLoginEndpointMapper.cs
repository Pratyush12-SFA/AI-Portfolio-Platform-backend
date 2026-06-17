using AIPortfolio.API.Common;
using AIPortfolio.API.Extensions;

namespace AIPortfolio.API.Endpoints.Auth;

internal sealed class GoogleLoginEndpointMapper : IEndpointMapper
{
    public void Map(IEndpointRouteBuilder endpointRouteBuilder)
    {
        ArgumentNullException.ThrowIfNull(endpointRouteBuilder);
        RouteGroupBuilder googleAuthGroup = endpointRouteBuilder.MapAuthGroup();

        googleAuthGroup.MapPost("google-login", GoogleLoginEndpoint.PostGoogleLogin)
            .WithDisplayName("google-login")
            .AllowAnonymous()
            .RequireRateLimiting("AuthPolicy")
            .WithDescription("Login using Google OAuth");
    }
}