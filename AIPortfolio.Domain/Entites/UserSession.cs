using AIPortfolio.Domain.Common;

namespace AIPortfolio.Domain.Entites;

public sealed class UserSession : AuditEntity
{
    public long Id { get; set; }
    public required long UserId { get; set; }
    public required long RefreshTokenId { get; set; }
    public string? UserAgent { get; set; }
    public string? DeviceName { get; set; }
    public string? Browser { get; set; }
    public bool IsActive { get; set; }
    public DateTime? LastActivity { get; set; }
    public DateTime? RevokedOn { get; set; }
    public string? RevokedBy { get; set; }
        
}