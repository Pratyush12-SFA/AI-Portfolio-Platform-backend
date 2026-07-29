using AIPortfolio.Domain.Common;

namespace AIPortfolio.Domain.Entites.Notification;

public sealed class Notification : BaseEntity
{
    public long UserId { get; set; }
    public string Type { get; set; } = string.Empty;    // Info, Success, Warning, Alert
    public string Title { get; set; } = string.Empty;
    public string? Message { get; set; }
    public bool IsRead { get; set; }
    public string? ActionUrl { get; set; }

    // Navigation
    public User User { get; set; } = null!;
}
