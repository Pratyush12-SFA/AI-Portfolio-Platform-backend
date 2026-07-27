using AIPortfolio.Domain.Common;

namespace AIPortfolio.Domain.Entites;

public sealed class User : AuditEntity
{
    public long Id { get; set; }

    public required string FullName { get; set; } 

    public required string Email { get; set; } 

    public string? PasswordHash { get; set; } 

    public bool IsActive { get; set; }
    public string? GoogleId { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public bool IsEmailVerified { get; set; }
    public string? ResetPasswordToken { get; set; }
    public DateTime? ResetPasswordExpiresAt { get; set; }
    public string? VerificationToken { get; set; }
    public DateTime? VerificationExpiresAt { get; set; }
}