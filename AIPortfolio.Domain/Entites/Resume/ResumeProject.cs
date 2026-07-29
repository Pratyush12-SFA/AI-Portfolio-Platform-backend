using System.Text.Json.Serialization;
using AIPortfolio.Domain.Common;

namespace AIPortfolio.Domain.Entites.Resume;

public sealed class ResumeProject : BaseEntity
{
    public long ResumeId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? TechStack { get; set; }
    public string? ProjectUrl { get; set; }
    public string? GithubUrl { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Role { get; set; }

    public string? Technologies
    {
        get => TechStack;
        set => TechStack = value;
    }

    public string? Url
    {
        get => ProjectUrl;
        set => ProjectUrl = value;
    }

    public int OrderIndex { get; set; }

    public int DisplayOrder
    {
        get => OrderIndex;
        set => OrderIndex = value;
    }

    public string? LiveDemoUrl
    {
        get => ProjectUrl;
        set => ProjectUrl = value;
    }

    public string? ThumbnailUrl { get; set; }

    // Navigation
    [JsonIgnore] public Resume? Resume { get; set; }
}