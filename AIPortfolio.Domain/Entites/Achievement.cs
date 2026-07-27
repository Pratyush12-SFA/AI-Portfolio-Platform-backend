using AIPortfolio.Domain.Common;

namespace AIPortfolio.Domain.Entites;

public sealed class Achievement : AuditEntity
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string Title { get; set; } = null!;
    public string? Issuer { get; set; }
    public DateTime? DateReceived { get; set; }
    public string? Description { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; } = true;
}
