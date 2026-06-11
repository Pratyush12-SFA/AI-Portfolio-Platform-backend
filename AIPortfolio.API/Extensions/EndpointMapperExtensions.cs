using AIPortfolio.API.Common;
using AIPortfolio.API.Endpoints.Auth;

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

        return services;
    }

    public static WebApplication MapEndpoints(
        this WebApplication app)
    {
        IEnumerable<IEndpointMapper> endpointMappers =
            app.Services.GetRequiredService<
                IEnumerable<IEndpointMapper>>();

        foreach (IEndpointMapper endpointMapper
                 in endpointMappers)
        {
            endpointMapper.Map(app);
        }

        return app;
    }
}