using AIPortfolio.API.Common;
using AIPortfolio.API.Endpoints.AI;
using AIPortfolio.API.Endpoints.Auth;
using AIPortfolio.API.Endpoints.Portfolio;

namespace AIPortfolio.API.Extensions;

public static class EndpointMapperExtensions
{
    public static IServiceCollection AddEndpointMappers(
        this IServiceCollection services)
    {
        services.AddSingleton<IEndpointMapper,
            LoginEndpointMapper>();

        services.AddSingleton<IEndpointMapper, RegisterEndpointMapper>();
        services.AddSingleton<IEndpointMapper, GoogleLoginEndpointMapper>();
        services.AddSingleton<IEndpointMapper, RefreshTokenEndpointMapper>();
        services.AddSingleton<IEndpointMapper, LogoutEndpointMapper>();
        services.AddSingleton<IEndpointMapper, AuthExtendedEndpointMapper>();
        services.AddSingleton<IEndpointMapper, PortfolioEndpointMapper>();
        services.AddSingleton<IEndpointMapper, ChatEndpointMapper>();
        services.AddSingleton<IEndpointMapper, ResumeEndpointMapper>();

        return services;
    }

    public static WebApplication MapEndpoints(
        this WebApplication app)
    {
        var endpointMappers =
            app.Services.GetRequiredService<
                IEnumerable<IEndpointMapper>>();

        foreach (var endpointMapper
                 in endpointMappers)
            endpointMapper.Map(app);

        return app;
    }
}