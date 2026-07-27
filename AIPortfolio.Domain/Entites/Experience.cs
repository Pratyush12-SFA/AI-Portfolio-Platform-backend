using AIPortfolio.Domain.Common;

namespace AIPortfolio.Domain.Entites;

public sealed class Experience : AuditEntity
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string CompanyName { get; set; } = null!;
    public string Designation { get; set; } = null!;
    public string EmploymentType { get; set; } = null!;
    public string? Location { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Description { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; } = true;
}
