using AIPortfolio.Domain.Common;

namespace AIPortfolio.Domain.Entites;

public sealed class Project : AuditEntity
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string Title { get; set; } = null!;
    public string ShortDescription { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? TechStack { get; set; }
    public string? GithubUrl { get; set; }
    public string? LiveDemoUrl { get; set; }
    public string? ThumbnailUrl { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsFeatured { get; set; }
    public bool IsActive { get; set; } = true;
}
