using AIPortfolio.Domain.Common;

namespace AIPortfolio.Domain.Entites;

public sealed class UserSession : BaseEntity
{
    public long UserId { get; set; }
    public long? RefreshTokenId { get; set; }
    public string DeviceName { get; set; } = string.Empty;
    public string IpAddress { get; set; } = string.Empty;
    public string? UserAgent { get; set; }
    public DateTime LastActiveAt { get; set; }
    public bool IsActive { get; set; } = true;

    // Frontend Mappings
    public string DeviceDetails => string.IsNullOrWhiteSpace(DeviceName) ? UserAgent ?? "Unknown Device" : DeviceName;

    public string DeviceType =>
        (UserAgent ?? "").Contains("Mobi", StringComparison.OrdinalIgnoreCase) ? "Mobile" : "Desktop";

    public bool IsCurrentActive { get; set; }

    // Navigation
    public User User { get; set; } = null!;
}