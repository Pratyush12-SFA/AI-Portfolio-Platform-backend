using System.Text.Json.Serialization;
using AIPortfolio.Domain.Common;

namespace AIPortfolio.Domain.Entites.Resume;

public sealed class ResumeVersion : BaseEntity
{
    public long ResumeId { get; set; }
    public int VersionNumber { get; set; }
    public string JsonSnapshot { get; set; } = string.Empty;
    public string? Label { get; set; }

    // Navigation
    [JsonIgnore] public Resume? Resume { get; set; }
}