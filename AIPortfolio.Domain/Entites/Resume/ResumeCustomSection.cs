using AIPortfolio.Domain.Common;

namespace AIPortfolio.Domain.Entites.Resume;

public sealed class ResumeCustomSection : BaseEntity
{
    public long ResumeId { get; set; }
    public string SectionTitle { get; set; } = string.Empty;
    public string SectionName { get => SectionTitle; set => SectionTitle = value; }
    public string Content { get; set; } = string.Empty;
    public int OrderIndex { get; set; }
    public int DisplayOrder { get => OrderIndex; set => OrderIndex = value; }
    public string Title { get => SectionTitle; set => SectionTitle = value; }

    // Navigation
    public Resume Resume { get; set; } = null!;
}
