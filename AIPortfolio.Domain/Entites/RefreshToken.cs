using AIPortfolio.Domain.Common;

namespace AIPortfolio.Domain.Entites;

public sealed class RefreshToken : BaseEntity
{
    public long UserId { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public DateTime? RevokedAt { get; set; }
    public string? RevokedBy { get; set; }
    public bool IsRevoked { get; set; }

    // Navigation
    public User User { get; set; } = null!;
}