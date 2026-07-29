using AIPortfolio.Domain.Common;

namespace AIPortfolio.Domain.Entites.Resume;

public sealed class ResumeCertification : BaseEntity
{
    public long ResumeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? IssuingOrganization { get; set; }
    public DateTime? IssueDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string? CredentialId { get; set; }
    public string? CredentialUrl { get; set; }
    public int OrderIndex { get; set; }
    public int DisplayOrder { get => OrderIndex; set => OrderIndex = value; }
    public DateTime? ExpirationDate { get => ExpiryDate; set => ExpiryDate = value; }
    public string? Issuer { get => IssuingOrganization; set => IssuingOrganization = value; }

    // Navigation
    public Resume Resume { get; set; } = null!;
}
