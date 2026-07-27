using System;

namespace AIPortfolio.Domain.Entites;

public sealed class PromptTemplate
{
    public long Id { get; set; }
    public required string Feature { get; set; }
    public int Version { get; set; }
    public required string SystemPrompt { get; set; }
    public required string UserPromptTemplate { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedOn { get; set; }
}
