using AIPortfolio.Domain.Common;

namespace AIPortfolio.Domain.Entites;

public sealed class AIChatSession : BaseEntity
{
    public long UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Context { get; set; }

    // Navigation
    public User User { get; set; } = null!;
    public ICollection<AIChatMessage> Messages { get; set; } = [];
}