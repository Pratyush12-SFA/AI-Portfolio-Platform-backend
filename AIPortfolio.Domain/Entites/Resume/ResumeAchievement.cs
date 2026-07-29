using AIPortfolio.Domain.Common;

namespace AIPortfolio.Domain.Entites.Resume;

public sealed class ResumeAchievement : BaseEntity
{
    public long ResumeId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? AchievedDate { get; set; }
    public int OrderIndex { get; set; }
    public int DisplayOrder { get => OrderIndex; set => OrderIndex = value; }
    public string? Date { get => AchievedDate?.ToString("yyyy-MM-dd"); set { if (DateTime.TryParse(value, out var d)) AchievedDate = d; else AchievedDate = null; } }

    // Navigation
    public Resume Resume { get; set; } = null!;
}
