using System;

namespace AIPortfolio.Domain.Entites;

public sealed class AIChatSession
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public required string Title { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }
}
