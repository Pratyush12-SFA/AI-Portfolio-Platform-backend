using AIPortfolio.Application.DTOs.Portfolio;
using AIPortfolio.Domain.Entites.Portfolio;
using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Portfolio.Commands.UpsertProfile;

public sealed record UpsertProfileCommand(
    string? CustomSlug,
    string? ProfileHeadline,
    string? ProfileSummary,
    string? ContactEmail,
    string? ContactPhone,
    string? Address,
    string? ProfilePictureUrl,
    string? BannerPictureUrl,
    string ThemeName,
    string? ThemeConfigJson,
    string? SEOTitle,
    string? SEODescription,
    string? SEOKeywords,
    long? SelectedThemeId,
    bool IsPublic,
    string? FullName,
    List<PortfolioSocialLink>? SocialLinks
) : IRequest<Result<UpsertProfileResponse>>;