namespace AIPortfolio.Domain.Common;

public abstract class AuditEntity
{
    public string CreatedBy { get; set; } = null!;
    public DateTime CreatedOn { get; set; }
    public string CreatedFromIp { get; set; } = null!;
}