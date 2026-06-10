using AIPortfolio.Application.Abstractions;
using AIPortfolio.Application.Feature.Auth;
using Microsoft.Extensions.DependencyInjection;

namespace AIPortfolio.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}