using System;

namespace AIPortfolio.Domain.Entites;

public sealed class AIChatMessage
{
    public long Id { get; set; }
    public long SessionId { get; set; }
    public required string Role { get; set; } // User, Assistant, System
    public required string Content { get; set; }
    public int InputTokens { get; set; }
    public int OutputTokens { get; set; }
    public DateTime CreatedOn { get; set; }
}
