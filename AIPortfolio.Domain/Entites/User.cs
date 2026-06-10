using AIPortfolio.Domain.Common;

namespace AIPortfolio.Domain.Entites;

public sealed class User : AuditEntity
{
    public long Id { get; set; }

    public required string FullName { get; set; } 

    public required string Email { get; set; } 

    public required string PasswordHash { get; set; } 

    public bool IsActive { get; set; }
}