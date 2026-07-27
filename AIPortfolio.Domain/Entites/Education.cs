using AIPortfolio.Domain.Common;

namespace AIPortfolio.Domain.Entites;

public sealed class Education : AuditEntity
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string Institution { get; set; } = null!;
    public string Degree { get; set; } = null!;
    public string? FieldOfStudy { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Grade { get; set; }
    public string? Description { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; } = true;
}
