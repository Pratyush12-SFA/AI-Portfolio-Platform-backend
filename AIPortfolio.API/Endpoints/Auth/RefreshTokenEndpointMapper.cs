using AIPortfolio.API.Common;
using AIPortfolio.API.Extensions;

namespace AIPortfolio.API.Endpoints.Auth;

internal sealed class RefreshTokenEndpointMapper : IEndpointMapper
{
    public void Map (IEndpointRouteBuilder endpointRouteBuilder)
    {
        ArgumentNullException.ThrowIfNull(endpointRouteBuilder);
        RouteGroupBuilder refreshTokenGroup = endpointRouteBuilder.MapAuthGroup();
        
        refreshTokenGroup.MapPost("refresh-token", RefreshTokenEndpoint.PostRefreshToken)
            .WithDisplayName("refresh-token")
            .WithName("refresh-token")
            .WithDescription("refresh-token details")
            .AllowAnonymous()
            .RequireAuthorization();
    }
}