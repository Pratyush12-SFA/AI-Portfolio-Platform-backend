using System.Text.Json.Serialization;
using AIPortfolio.Domain.Common;

namespace AIPortfolio.Domain.Entites.Resume;

public sealed class ResumeEducation : BaseEntity
{
    public long ResumeId { get; set; }
    public string Institution { get; set; } = string.Empty;
    public string Degree { get; set; } = string.Empty;
    public string? FieldOfStudy { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsCurrent { get; set; }
    public string? Grade { get; set; }
    public string? Description { get; set; }
    public int OrderIndex { get; set; }

    public int DisplayOrder
    {
        get => OrderIndex;
        set => OrderIndex = value;
    }

    // Navigation
    [JsonIgnore] public Resume? Resume { get; set; }
}