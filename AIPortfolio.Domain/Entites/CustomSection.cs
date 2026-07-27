using AIPortfolio.Domain.Common;

namespace AIPortfolio.Domain.Entites;

public sealed class CustomSection : AuditEntity
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string SectionTitle { get; set; } = null!;
    public string Content { get; set; } = null!;
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; } = true;
}
