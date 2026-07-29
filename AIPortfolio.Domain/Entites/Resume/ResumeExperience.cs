using System.Text.Json.Serialization;
using AIPortfolio.Domain.Common;

namespace AIPortfolio.Domain.Entites.Resume;

public sealed class ResumeExperience : BaseEntity
{
    public long ResumeId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string JobTitle { get; set; } = string.Empty;
    public string? Location { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsCurrent { get; set; }
    public string? Description { get; set; }
    public string? Responsibilities { get; set; }

    public string? Designation
    {
        get => JobTitle;
        set => JobTitle = value ?? string.Empty;
    }

    public string? EmploymentType { get; set; }
    public int OrderIndex { get; set; }

    public int DisplayOrder
    {
        get => OrderIndex;
        set => OrderIndex = value;
    }

    public string Company
    {
        get => CompanyName;
        set => CompanyName = value ?? string.Empty;
    }

    public string Position
    {
        get => JobTitle;
        set => JobTitle = value ?? string.Empty;
    }

    // Navigation
    [JsonIgnore] public Resume? Resume { get; set; }
}