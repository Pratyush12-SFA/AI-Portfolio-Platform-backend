namespace AIPortfolio.Infrastructure.Configurations;

public sealed class GeminiSettings
{
    public string GeminiApiKey { get; set; } = null!;
    public string DefaultModel { get; set; } = "gemini-3.6-flash";
}