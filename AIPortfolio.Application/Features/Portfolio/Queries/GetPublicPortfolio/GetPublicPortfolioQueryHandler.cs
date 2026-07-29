using AIPortfolio.Application.Abstractions;
using AIPortfolio.Application.DTOs.Portfolio;
using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Portfolio.Queries.GetPublicPortfolio;

internal sealed class GetPublicPortfolioQueryHandler(
    IPortfolioRepository repository) : IRequestHandler<GetPublicPortfolioQuery, Result<PublicPortfolioResponse>>
{
    public async Task<Result<PublicPortfolioResponse>> Handle(GetPublicPortfolioQuery query,
        CancellationToken cancellationToken)
    {
        var profile = await repository.GetProfileBySlugAsync(query.Slug);
        if (profile is null) return Result.NotFound("Portfolio not found.");

        var userId = profile.UserId;

        var educations = await repository.GetEducationsByUserIdAsync(userId);
        var experiences = await repository.GetExperiencesByUserIdAsync(userId);
        var projects = await repository.GetProjectsByUserIdAsync(userId);
        var skills = await repository.GetSkillsByUserIdAsync(userId);
        var certifications = await repository.GetCertificationsByUserIdAsync(userId);
        var achievements = await repository.GetAchievementsByUserIdAsync(userId);
        var languages = await repository.GetLanguagesByUserIdAsync(userId);
        var socialLinks = await repository.GetSocialLinksByUserIdAsync(userId);
        var customSections = await repository.GetCustomSectionsByUserIdAsync(userId);

        return new PublicPortfolioResponse(
            profile,
            educations,
            experiences,
            projects,
            skills,
            certifications,
            achievements,
            languages,
            socialLinks,
            customSections
        );
    }
}