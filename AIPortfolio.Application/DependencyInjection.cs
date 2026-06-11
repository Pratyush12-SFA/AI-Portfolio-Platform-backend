using AIPortfolio.Application.Abstractions;
using AIPortfolio.Application.Feature.Auth;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;

namespace AIPortfolio.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddValidatorsFromAssembly(
            typeof(DependencyInjection).Assembly);
        services.AddScoped<IGoogleAuthService, GoogleAuthService>();

        return services;
    }
}