using AIPortfolio.Domain.Common;

namespace AIPortfolio.Domain.Entites;

public sealed class Profile : AuditEntity
{
    public long UserId { get; set; }
    public string? Headline { get; set; }
    public string? Summary { get; set; }
    public string? PhoneNumber { get; set; }
    public string? ContactEmail { get; set; }
    public string? Address { get; set; }
    public string ThemeName { get; set; } = "ModernDark";
    public string? CustomSlug { get; set; }
    public bool IsDarkModePreferred { get; set; } = true;
}
