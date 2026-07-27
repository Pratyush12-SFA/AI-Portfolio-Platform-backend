using AIPortfolio.Domain.Common;

namespace AIPortfolio.Domain.Entites;

public sealed class SocialLink : AuditEntity
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string PlatformName { get; set; } = null!;
    public string Url { get; set; } = null!;
    public bool IsActive { get; set; } = true;
}
