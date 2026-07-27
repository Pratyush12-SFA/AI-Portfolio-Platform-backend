using AIPortfolio.Domain.Common;

namespace AIPortfolio.Domain.Entites;

public sealed class Certification : AuditEntity
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string Title { get; set; } = null!;
    public string IssuingOrganization { get; set; } = null!;
    public string? CertificateUrl { get; set; }
    public DateTime IssueDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; } = true;
}
