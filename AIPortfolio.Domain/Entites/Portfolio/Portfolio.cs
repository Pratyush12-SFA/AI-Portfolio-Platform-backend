using System.Text.Json.Serialization;
using AIPortfolio.Domain.Common;

namespace AIPortfolio.Domain.Entites.Portfolio;

public sealed class Portfolio : BaseEntity
{
    public long UserId { get; set; }
    public string CustomSlug { get; set; } = string.Empty;
    public string? ProfileHeadline { get; set; }
    public string? ProfileSummary { get; set; }
    public string? ContactEmail { get; set; }
    public string? ContactPhone { get; set; }
    public string? Address { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public string? BannerPictureUrl { get; set; }

    // Theme (stored as plain string — no FK catalog)
    public string ThemeName { get; set; } = "default";
    public string? ThemeConfigJson { get; set; }

    // SEO (folded in directly — no separate table)
    public string? SEOTitle { get; set; }
    public string? SEODescription { get; set; }
    public string? SEOKeywords { get; set; }

    public long? SelectedThemeId { get; set; }
    public bool IsPublic { get; set; }

    // Frontend compatibility properties (mapped to DB fields, ignored in EF Core)
    [JsonPropertyName("Headline")]
    public string? Headline
    {
        get => ProfileHeadline;
        set => ProfileHeadline = value;
    }

    [JsonPropertyName("Summary")]
    public string? Summary
    {
        get => ProfileSummary;
        set => ProfileSummary = value;
    }

    [JsonPropertyName("PhoneNumber")]
    public string? PhoneNumber
    {
        get => ContactPhone;
        set => ContactPhone = value;
    }

    [JsonPropertyName("FullName")] public string? FullName { get; set; }

    // Navigation
    public ICollection<PortfolioSocialLink> SocialLinks { get; set; } = [];
    public ICollection<RecruiterMessage> RecruiterMessages { get; set; } = [];
}