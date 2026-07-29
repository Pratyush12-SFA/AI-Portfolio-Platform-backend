using AIPortfolio.Domain.Common;

namespace AIPortfolio.Domain.Entites;

public sealed class User : BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PasswordHash { get; set; }
    public bool IsActive { get; set; } = true;
    public string? GoogleId { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public bool IsEmailVerified { get; set; }
    public string? ResetPasswordToken { get; set; }
    public DateTime? ResetPasswordExpiresAt { get; set; }
    public string? VerificationToken { get; set; }
    public DateTime? VerificationExpiresAt { get; set; }

    // Navigation
    public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
    public ICollection<UserSession> Sessions { get; set; } = [];
}