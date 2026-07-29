using AIPortfolio.Domain.Common;

namespace AIPortfolio.Domain.Entites;

public sealed class AIChatMessage : BaseEntity
{
    public long SessionId { get; set; }
    public string Role { get; set; } = string.Empty; // "user" | "assistant"
    public string Content { get; set; } = string.Empty;
    public int? TokensUsed { get; set; }
    public int? InputTokens { get; set; }
    public int? OutputTokens { get; set; }

    // Navigation
    public AIChatSession Session { get; set; } = null!;
}