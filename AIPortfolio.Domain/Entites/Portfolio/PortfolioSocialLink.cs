using AIPortfolio.Domain.Common;

namespace AIPortfolio.Domain.Entites.Portfolio;

public sealed class PortfolioSocialLink : BaseEntity
{
    public long PortfolioId { get; set; }
    public string Platform { get; set; } = string.Empty;

    public string PlatformName
    {
        get => Platform;
        set => Platform = value;
    }

    public string Url { get; set; } = string.Empty;
    public int OrderIndex { get; set; }

    public int DisplayOrder
    {
        get => OrderIndex;
        set => OrderIndex = value;
    }

    // Navigation
    public Portfolio Portfolio { get; set; } = null!;
}