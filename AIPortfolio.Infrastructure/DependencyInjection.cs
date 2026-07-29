using AIPortfolio.Application.Abstraction;
using AIPortfolio.Application.Abstractions;
using AIPortfolio.Infrastructure.AI;
using AIPortfolio.Infrastructure.Authentication;
using AIPortfolio.Infrastructure.Configurations;
using AIPortfolio.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AIPortfolio.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<JwtSettings>(
            configuration.GetSection("JwtSettings"));

        services.Configure<GeminiSettings>(
            configuration.GetSection("GeminiSettings"));

        services.AddScoped<IDateHandler, DateHandler>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        
        services.AddScoped<IUserInfoAccessor, UserInfoAccessor>();
        services.AddScoped<IRefreshTokenGenerator, RefreshTokenGenerator>();
        services.AddScoped<ICookieService, CookieService>();

        services.AddHttpClient<IAIProvider, GeminiProvider>();
        services.AddScoped<IAIUsageService, AIUsageService>();
        services.AddScoped<IChatService, ChatService>();
        services.AddScoped<IResumeAIService, ResumeAIService>();

        return services;
    }
}