namespace AIPortfolio.Domain.Common;

public abstract class BaseEntity
{
    public long Id { get; set; }

    public DateTime CreatedOn { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string CreatedFromIp { get; set; } = string.Empty;

    public DateTime? UpdatedOn { get; set; }
    public string? UpdatedBy { get; set; }
    public string? UpdatedFromIp { get; set; }

    public DateTime? DeletedOn { get; set; }
    public string? DeletedBy { get; set; }
    public string? DeletedFromIp { get; set; }
    public bool IsDeleted { get; set; }

    public byte[] RowVersion { get; set; } = [];
}
