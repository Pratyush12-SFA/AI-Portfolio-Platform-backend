using System;

namespace AIPortfolio.Domain.Entites;

public sealed class AIUsage
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public required string Feature { get; set; }
    public required string Model { get; set; }
    public required string PromptType { get; set; }
    public int InputTokens { get; set; }
    public int OutputTokens { get; set; }
    public decimal EstimatedCost { get; set; }
    public int DurationMs { get; set; }
    public required string Status { get; set; }
    public DateTime CreatedOn { get; set; }
}
