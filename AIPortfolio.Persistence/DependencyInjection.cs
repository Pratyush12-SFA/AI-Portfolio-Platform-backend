using AIPortfolio.Application.Abstractions;
using AIPortfolio.Persistence.Connections;
using AIPortfolio.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AIPortfolio.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton(
            new DapperContext(
                configuration.GetConnectionString("DefaultConnection")));
            


        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IUserSessionRepository, UserSessionRepository>();
        services.AddScoped<IPortfolioRepository, PortfolioRepository>();
        services.AddScoped<IPromptRepository, PromptRepository>();
        services.AddScoped<IChatRepository, ChatRepository>();
        services.AddScoped<IAIUsageRepository, AIUsageRepository>();

        return services;
    }
}