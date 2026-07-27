using AIPortfolio.API.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AIPortfolio.API.Endpoints.Portfolio;

internal sealed class PortfolioEndpointMapper : IEndpointMapper
{
    public void Map(IEndpointRouteBuilder endpointRouteBuilder)
    {
        ArgumentNullException.ThrowIfNull(endpointRouteBuilder);

        // Profile Group
        var profileGroup = endpointRouteBuilder.MapGroup("/api/profile")
            .WithTags("Profile")
            .RequireAuthorization();

        profileGroup.MapGet("", PortfolioEndpoints.GetProfile)
            .WithDisplayName("Get Profile")
            .WithName("GetProfile");

        profileGroup.MapPost("", PortfolioEndpoints.UpsertProfile)
            .WithDisplayName("Upsert Profile")
            .WithName("UpsertProfile");

        // Resume Group
        var resumeGroup = endpointRouteBuilder.MapGroup("/api/resume")
            .WithTags("Resume Builder")
            .RequireAuthorization();

        // Education
        resumeGroup.MapGet("education", PortfolioEndpoints.GetEducation)
            .WithName("GetEducation");
        resumeGroup.MapPost("education", PortfolioEndpoints.UpsertEducation)
            .WithName("UpsertEducation");
        resumeGroup.MapDelete("education/{id:long}", PortfolioEndpoints.DeleteEducation)
            .WithName("DeleteEducation");

        // Experience
        resumeGroup.MapGet("experience", PortfolioEndpoints.GetExperience)
            .WithName("GetExperience");
        resumeGroup.MapPost("experience", PortfolioEndpoints.UpsertExperience)
            .WithName("UpsertExperience");
        resumeGroup.MapDelete("experience/{id:long}", PortfolioEndpoints.DeleteExperience)
            .WithName("DeleteExperience");

        // Projects
        resumeGroup.MapGet("projects", PortfolioEndpoints.GetProjects)
            .WithName("GetProjects");
        resumeGroup.MapPost("projects", PortfolioEndpoints.UpsertProject)
            .WithName("UpsertProject");
        resumeGroup.MapDelete("projects/{id:long}", PortfolioEndpoints.DeleteProject)
            .WithName("DeleteProject");

        // Skills
        resumeGroup.MapGet("skills", PortfolioEndpoints.GetSkills)
            .WithName("GetSkills");
        resumeGroup.MapPost("skills", PortfolioEndpoints.UpsertSkill)
            .WithName("UpsertSkill");
        resumeGroup.MapDelete("skills/{id:long}", PortfolioEndpoints.DeleteSkill)
            .WithName("DeleteSkill");

        // Certifications
        resumeGroup.MapGet("certifications", PortfolioEndpoints.GetCertifications)
            .WithName("GetCertifications");
        resumeGroup.MapPost("certifications", PortfolioEndpoints.UpsertCertification)
            .WithName("UpsertCertification");
        resumeGroup.MapDelete("certifications/{id:long}", PortfolioEndpoints.DeleteCertification)
            .WithName("DeleteCertification");

        // Achievements
        resumeGroup.MapGet("achievements", PortfolioEndpoints.GetAchievements)
            .WithName("GetAchievements");
        resumeGroup.MapPost("achievements", PortfolioEndpoints.UpsertAchievement)
            .WithName("UpsertAchievement");
        resumeGroup.MapDelete("achievements/{id:long}", PortfolioEndpoints.DeleteAchievement)
            .WithName("DeleteAchievement");

        // Languages
        resumeGroup.MapGet("languages", PortfolioEndpoints.GetLanguages)
            .WithName("GetLanguages");
        resumeGroup.MapPost("languages", PortfolioEndpoints.UpsertLanguage)
            .WithName("UpsertLanguage");
        resumeGroup.MapDelete("languages/{id:long}", PortfolioEndpoints.DeleteLanguage)
            .WithName("DeleteLanguage");

        // Social Links
        resumeGroup.MapGet("social-links", PortfolioEndpoints.GetSocialLinks)
            .WithName("GetSocialLinks");
        resumeGroup.MapPost("social-links", PortfolioEndpoints.UpsertSocialLink)
            .WithName("UpsertSocialLink");
        resumeGroup.MapDelete("social-links/{id:long}", PortfolioEndpoints.DeleteSocialLink)
            .WithName("DeleteSocialLink");

        // Custom Sections
        resumeGroup.MapGet("custom-sections", PortfolioEndpoints.GetCustomSections)
            .WithName("GetCustomSections");
        resumeGroup.MapPost("custom-sections", PortfolioEndpoints.UpsertCustomSection)
            .WithName("UpsertCustomSection");
        resumeGroup.MapDelete("custom-sections/{id:long}", PortfolioEndpoints.DeleteCustomSection)
            .WithName("DeleteCustomSection");

        // Public Portfolio Group
        var publicGroup = endpointRouteBuilder.MapGroup("/api/portfolio")
            .WithTags("Public Portfolio")
            .AllowAnonymous();

        publicGroup.MapGet("{slug}", PortfolioEndpoints.GetPublicPortfolio)
            .WithName("GetPublicPortfolio");

        publicGroup.MapPost("contact", PortfolioEndpoints.SendPortfolioMessage)
            .WithName("SendPortfolioMessage");
    }
}
