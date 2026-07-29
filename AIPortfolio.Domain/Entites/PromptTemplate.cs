using AIPortfolio.Domain.Common;

namespace AIPortfolio.Domain.Entites;

public sealed class PromptTemplate : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Feature { get; set; } = string.Empty;
    public string SystemPrompt { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public int Version { get; set; } = 1;
}