using AIPortfolio.Domain.Common;

namespace AIPortfolio.Domain.Entites.Resume;

public sealed class ResumeTemplate : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? ThumbnailUrl { get; set; }
    public string? ConfigJson { get; set; }
    public bool IsActive { get; set; } = true;
}
