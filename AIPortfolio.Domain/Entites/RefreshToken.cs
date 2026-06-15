using AIPortfolio.Domain.Common;

namespace AIPortfolio.Domain.Entites;

public sealed class RefreshToken : AuditEntity
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public required string Token {get; set;}
    public DateTime ExpiresAt { get; set; }
    public DateTime RevokedAt { get; set; }
    public string? RevokedBy { get; set; }
    public  bool IsRevoked { get; set; }
}