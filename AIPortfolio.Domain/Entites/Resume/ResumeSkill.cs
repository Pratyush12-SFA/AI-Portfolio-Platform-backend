using AIPortfolio.Domain.Common;

namespace AIPortfolio.Domain.Entites.Resume;

public sealed class ResumeSkill : BaseEntity
{
    public long ResumeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Category { get; set; }
    public string? ProficiencyLevel { get; set; }
    public int OrderIndex { get; set; }
    public int DisplayOrder { get => OrderIndex; set => OrderIndex = value; }

    // Navigation
    public Resume Resume { get; set; } = null!;
}
