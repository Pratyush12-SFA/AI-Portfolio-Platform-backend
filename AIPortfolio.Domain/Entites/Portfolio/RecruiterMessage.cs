using AIPortfolio.Domain.Common;

namespace AIPortfolio.Domain.Entites.Portfolio;

public sealed class RecruiterMessage : BaseEntity
{
    public long PortfolioId { get; set; }
    public string SenderName { get; set; } = string.Empty;
    public string SenderEmail { get; set; } = string.Empty;
    public string? SenderCompany { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; }

    // Navigation
    public Portfolio Portfolio { get; set; } = null!;
}