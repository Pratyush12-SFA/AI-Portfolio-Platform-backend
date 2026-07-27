using AIPortfolio.Domain.Common;

namespace AIPortfolio.Domain.Entites;

public sealed class Language : AuditEntity
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string LanguageName { get; set; } = null!;
    public string Proficiency { get; set; } = null!;
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; } = true;
}
