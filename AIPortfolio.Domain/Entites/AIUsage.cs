using AIPortfolio.Domain.Common;

namespace AIPortfolio.Domain.Entites;

public sealed class AIUsage : BaseEntity
{
    public long UserId { get; set; }
    public string FeatureName { get; set; } = string.Empty;

    public string Feature
    {
        get => FeatureName;
        set => FeatureName = value;
    }

    public string? Model { get; set; }
    public string? PromptType { get; set; }
    public int TokensUsed { get; set; }
    public int InputTokens { get; set; }
    public int OutputTokens { get; set; }
    public decimal EstimatedCost { get; set; }
    public long DurationMs { get; set; }
    public string Status { get; set; } = "Success";
    public int CreditsUsed { get; set; }
    public int RemainingCredits { get; set; }
    public DateTime UsedAt { get; set; }

    // Navigation
    public User User { get; set; } = null!;
}