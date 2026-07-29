using AIPortfolio.Application.Abstractions;
using AIPortfolio.Domain.Entites.Portfolio;
using AIPortfolio.Domain.Entites.Resume;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AIPortfolio.API.Endpoints.Portfolio;

internal static class PortfolioEndpoints
{
    // PROFILE
    public static async Task<IResult> GetProfile(
        IPortfolioRepository repository,
        IUserInfoAccessor userInfoAccessor)
    {
        if (!userInfoAccessor.IsAuthenticated) return Results.Unauthorized();
        
        var profile = await repository.GetProfileByUserIdAsync(userInfoAccessor.UserId);
        if (profile is null)
        {
            // Return empty profile with default settings
            profile = new Domain.Entites.Portfolio.Portfolio
            {
                UserId = userInfoAccessor.UserId,
                CustomSlug = "user-" + userInfoAccessor.UserId,
                IsPublic = true,
                CreatedBy = userInfoAccessor.Email ?? "system",
                CreatedFromIp = userInfoAccessor.GetRemoteIp() ?? "127.0.0.1",
                RowVersion = new byte[8]
            };
        }
        return Results.Ok(profile);
    }

    public static async Task<IResult> UpsertProfile(
        Domain.Entites.Portfolio.Portfolio profileRequest,
        IPortfolioRepository repository,
        IUserInfoAccessor userInfoAccessor)
    {
        if (!userInfoAccessor.IsAuthenticated) return Results.Unauthorized();

        profileRequest.UserId = userInfoAccessor.UserId;
        profileRequest.CreatedBy = userInfoAccessor.Email ?? "system";
        profileRequest.CreatedFromIp = userInfoAccessor.GetRemoteIp() ?? "127.0.0.1";
        profileRequest.RowVersion = new byte[8];

        await repository.UpsertProfileAsync(profileRequest);
        return Results.Ok(new { success = true, message = "Profile updated successfully." });
    }

    // EDUCATION
    public static async Task<IResult> GetEducation(
        IPortfolioRepository repository,
        IUserInfoAccessor userInfoAccessor)
    {
        if (!userInfoAccessor.IsAuthenticated) return Results.Unauthorized();
        var data = await repository.GetEducationsByUserIdAsync(userInfoAccessor.UserId);
        return Results.Ok(data);
    }

    public static async Task<IResult> UpsertEducation(
        ResumeEducation education,
        IPortfolioRepository repository,
        IUserInfoAccessor userInfoAccessor)
    {
        if (!userInfoAccessor.IsAuthenticated) return Results.Unauthorized();

        education.CreatedBy = userInfoAccessor.Email ?? "system";
        education.CreatedFromIp = userInfoAccessor.GetRemoteIp() ?? "127.0.0.1";
        education.RowVersion = new byte[8];

        long id = await repository.UpsertEducationAsync(education);
        return Results.Ok(new { success = true, id });
    }

    public static async Task<IResult> DeleteEducation(
        long id,
        IPortfolioRepository repository,
        IUserInfoAccessor userInfoAccessor)
    {
        if (!userInfoAccessor.IsAuthenticated) return Results.Unauthorized();
        await repository.DeleteEducationAsync(id, userInfoAccessor.UserId);
        return Results.Ok(new { success = true });
    }

    // EXPERIENCE
    public static async Task<IResult> GetExperience(
        IPortfolioRepository repository,
        IUserInfoAccessor userInfoAccessor)
    {
        if (!userInfoAccessor.IsAuthenticated) return Results.Unauthorized();
        var data = await repository.GetExperiencesByUserIdAsync(userInfoAccessor.UserId);
        return Results.Ok(data);
    }

    public static async Task<IResult> UpsertExperience(
        ResumeExperience experience,
        IPortfolioRepository repository,
        IUserInfoAccessor userInfoAccessor)
    {
        if (!userInfoAccessor.IsAuthenticated) return Results.Unauthorized();

        experience.CreatedBy = userInfoAccessor.Email ?? "system";
        experience.CreatedFromIp = userInfoAccessor.GetRemoteIp() ?? "127.0.0.1";
        experience.RowVersion = new byte[8];

        long id = await repository.UpsertExperienceAsync(experience);
        return Results.Ok(new { success = true, id });
    }

    public static async Task<IResult> DeleteExperience(
        long id,
        IPortfolioRepository repository,
        IUserInfoAccessor userInfoAccessor)
    {
        if (!userInfoAccessor.IsAuthenticated) return Results.Unauthorized();
        await repository.DeleteExperienceAsync(id, userInfoAccessor.UserId);
        return Results.Ok(new { success = true });
    }

    // PROJECTS
    public static async Task<IResult> GetProjects(
        IPortfolioRepository repository,
        IUserInfoAccessor userInfoAccessor)
    {
        if (!userInfoAccessor.IsAuthenticated) return Results.Unauthorized();
        var data = await repository.GetProjectsByUserIdAsync(userInfoAccessor.UserId);
        return Results.Ok(data);
    }

    public static async Task<IResult> UpsertProject(
        ResumeProject project,
        IPortfolioRepository repository,
        IUserInfoAccessor userInfoAccessor)
    {
        if (!userInfoAccessor.IsAuthenticated) return Results.Unauthorized();

        project.CreatedBy = userInfoAccessor.Email ?? "system";
        project.CreatedFromIp = userInfoAccessor.GetRemoteIp() ?? "127.0.0.1";
        project.RowVersion = new byte[8];

        long id = await repository.UpsertProjectAsync(project);
        return Results.Ok(new { success = true, id });
    }

    public static async Task<IResult> DeleteProject(
        long id,
        IPortfolioRepository repository,
        IUserInfoAccessor userInfoAccessor)
    {
        if (!userInfoAccessor.IsAuthenticated) return Results.Unauthorized();
        await repository.DeleteProjectAsync(id, userInfoAccessor.UserId);
        return Results.Ok(new { success = true });
    }

    // SKILLS
    public static async Task<IResult> GetSkills(
        IPortfolioRepository repository,
        IUserInfoAccessor userInfoAccessor)
    {
        if (!userInfoAccessor.IsAuthenticated) return Results.Unauthorized();
        var data = await repository.GetSkillsByUserIdAsync(userInfoAccessor.UserId);
        return Results.Ok(data);
    }

    public static async Task<IResult> UpsertSkill(
        ResumeSkill skill,
        IPortfolioRepository repository,
        IUserInfoAccessor userInfoAccessor)
    {
        if (!userInfoAccessor.IsAuthenticated) return Results.Unauthorized();

        skill.CreatedBy = userInfoAccessor.Email ?? "system";
        skill.CreatedFromIp = userInfoAccessor.GetRemoteIp() ?? "127.0.0.1";
        skill.RowVersion = new byte[8];

        long id = await repository.UpsertSkillAsync(skill);
        return Results.Ok(new { success = true, id });
    }

    public static async Task<IResult> DeleteSkill(
        long id,
        IPortfolioRepository repository,
        IUserInfoAccessor userInfoAccessor)
    {
        if (!userInfoAccessor.IsAuthenticated) return Results.Unauthorized();
        await repository.DeleteSkillAsync(id, userInfoAccessor.UserId);
        return Results.Ok(new { success = true });
    }

    // CERTIFICATIONS
    public static async Task<IResult> GetCertifications(
        IPortfolioRepository repository,
        IUserInfoAccessor userInfoAccessor)
    {
        if (!userInfoAccessor.IsAuthenticated) return Results.Unauthorized();
        var data = await repository.GetCertificationsByUserIdAsync(userInfoAccessor.UserId);
        return Results.Ok(data);
    }

    public static async Task<IResult> UpsertCertification(
        ResumeCertification certification,
        IPortfolioRepository repository,
        IUserInfoAccessor userInfoAccessor)
    {
        if (!userInfoAccessor.IsAuthenticated) return Results.Unauthorized();

        certification.CreatedBy = userInfoAccessor.Email ?? "system";
        certification.CreatedFromIp = userInfoAccessor.GetRemoteIp() ?? "127.0.0.1";
        certification.RowVersion = new byte[8];

        long id = await repository.UpsertCertificationAsync(certification);
        return Results.Ok(new { success = true, id });
    }

    public static async Task<IResult> DeleteCertification(
        long id,
        IPortfolioRepository repository,
        IUserInfoAccessor userInfoAccessor)
    {
        if (!userInfoAccessor.IsAuthenticated) return Results.Unauthorized();
        await repository.DeleteCertificationAsync(id, userInfoAccessor.UserId);
        return Results.Ok(new { success = true });
    }

    // ACHIEVEMENTS
    public static async Task<IResult> GetAchievements(
        IPortfolioRepository repository,
        IUserInfoAccessor userInfoAccessor)
    {
        if (!userInfoAccessor.IsAuthenticated) return Results.Unauthorized();
        var data = await repository.GetAchievementsByUserIdAsync(userInfoAccessor.UserId);
        return Results.Ok(data);
    }

    public static async Task<IResult> UpsertAchievement(
        ResumeAchievement achievement,
        IPortfolioRepository repository,
        IUserInfoAccessor userInfoAccessor)
    {
        if (!userInfoAccessor.IsAuthenticated) return Results.Unauthorized();

        achievement.CreatedBy = userInfoAccessor.Email ?? "system";
        achievement.CreatedFromIp = userInfoAccessor.GetRemoteIp() ?? "127.0.0.1";
        achievement.RowVersion = new byte[8];

        long id = await repository.UpsertAchievementAsync(achievement);
        return Results.Ok(new { success = true, id });
    }

    public static async Task<IResult> DeleteAchievement(
        long id,
        IPortfolioRepository repository,
        IUserInfoAccessor userInfoAccessor)
    {
        if (!userInfoAccessor.IsAuthenticated) return Results.Unauthorized();
        await repository.DeleteAchievementAsync(id, userInfoAccessor.UserId);
        return Results.Ok(new { success = true });
    }

    // LANGUAGES
    public static async Task<IResult> GetLanguages(
        IPortfolioRepository repository,
        IUserInfoAccessor userInfoAccessor)
    {
        if (!userInfoAccessor.IsAuthenticated) return Results.Unauthorized();
        var data = await repository.GetLanguagesByUserIdAsync(userInfoAccessor.UserId);
        return Results.Ok(data);
    }

    public static async Task<IResult> UpsertLanguage(
        ResumeLanguage language,
        IPortfolioRepository repository,
        IUserInfoAccessor userInfoAccessor)
    {
        if (!userInfoAccessor.IsAuthenticated) return Results.Unauthorized();

        language.CreatedBy = userInfoAccessor.Email ?? "system";
        language.CreatedFromIp = userInfoAccessor.GetRemoteIp() ?? "127.0.0.1";
        language.RowVersion = new byte[8];

        long id = await repository.UpsertLanguageAsync(language);
        return Results.Ok(new { success = true, id });
    }

    public static async Task<IResult> DeleteLanguage(
        long id,
        IPortfolioRepository repository,
        IUserInfoAccessor userInfoAccessor)
    {
        if (!userInfoAccessor.IsAuthenticated) return Results.Unauthorized();
        await repository.DeleteLanguageAsync(id, userInfoAccessor.UserId);
        return Results.Ok(new { success = true });
    }

    // SOCIAL LINKS
    public static async Task<IResult> GetSocialLinks(
        IPortfolioRepository repository,
        IUserInfoAccessor userInfoAccessor)
    {
        if (!userInfoAccessor.IsAuthenticated) return Results.Unauthorized();
        var data = await repository.GetSocialLinksByUserIdAsync(userInfoAccessor.UserId);
        return Results.Ok(data);
    }

    public static async Task<IResult> UpsertSocialLink(
        PortfolioSocialLink socialLink,
        IPortfolioRepository repository,
        IUserInfoAccessor userInfoAccessor)
    {
        if (!userInfoAccessor.IsAuthenticated) return Results.Unauthorized();

        socialLink.CreatedBy = userInfoAccessor.Email ?? "system";
        socialLink.CreatedFromIp = userInfoAccessor.GetRemoteIp() ?? "127.0.0.1";
        socialLink.RowVersion = new byte[8];

        long id = await repository.UpsertSocialLinkAsync(socialLink);
        return Results.Ok(new { success = true, id });
    }

    public static async Task<IResult> DeleteSocialLink(
        long id,
        IPortfolioRepository repository,
        IUserInfoAccessor userInfoAccessor)
    {
        if (!userInfoAccessor.IsAuthenticated) return Results.Unauthorized();
        await repository.DeleteSocialLinkAsync(id, userInfoAccessor.UserId);
        return Results.Ok(new { success = true });
    }

    // CUSTOM SECTIONS
    public static async Task<IResult> GetCustomSections(
        IPortfolioRepository repository,
        IUserInfoAccessor userInfoAccessor)
    {
        if (!userInfoAccessor.IsAuthenticated) return Results.Unauthorized();
        var data = await repository.GetCustomSectionsByUserIdAsync(userInfoAccessor.UserId);
        return Results.Ok(data);
    }

    public static async Task<IResult> UpsertCustomSection(
        ResumeCustomSection customSection,
        IPortfolioRepository repository,
        IUserInfoAccessor userInfoAccessor)
    {
        if (!userInfoAccessor.IsAuthenticated) return Results.Unauthorized();

        customSection.CreatedBy = userInfoAccessor.Email ?? "system";
        customSection.CreatedFromIp = userInfoAccessor.GetRemoteIp() ?? "127.0.0.1";
        customSection.RowVersion = new byte[8];

        long id = await repository.UpsertCustomSectionAsync(customSection);
        return Results.Ok(new { success = true, id });
    }

    public static async Task<IResult> DeleteCustomSection(
        long id,
        IPortfolioRepository repository,
        IUserInfoAccessor userInfoAccessor)
    {
        if (!userInfoAccessor.IsAuthenticated) return Results.Unauthorized();
        await repository.DeleteCustomSectionAsync(id, userInfoAccessor.UserId);
        return Results.Ok(new { success = true });
    }

    // PUBLIC PORTFOLIO retrieval
    public static async Task<IResult> GetPublicPortfolio(
        string slug,
        IPortfolioRepository repository)
    {
        var profile = await repository.GetProfileBySlugAsync(slug);
        if (profile is null)
        {
            return Results.NotFound(new { message = "Portfolio not found." });
        }

        long userId = profile.UserId;

        var educations = await repository.GetEducationsByUserIdAsync(userId);
        var experiences = await repository.GetExperiencesByUserIdAsync(userId);
        var projects = await repository.GetProjectsByUserIdAsync(userId);
        var skills = await repository.GetSkillsByUserIdAsync(userId);
        var certifications = await repository.GetCertificationsByUserIdAsync(userId);
        var achievements = await repository.GetAchievementsByUserIdAsync(userId);
        var languages = await repository.GetLanguagesByUserIdAsync(userId);
        var socialLinks = await repository.GetSocialLinksByUserIdAsync(userId);
        var customSections = await repository.GetCustomSectionsByUserIdAsync(userId);

        return Results.Ok(new
        {
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
        });
    }

    public static IResult SendPortfolioMessage(
        [FromBody] ContactMessageRequest request)
    {
        // Mock contact email sending
        Console.WriteLine($"[CONTACT MESSAGE] From: {request.Name} ({request.Email}) - Subject: {request.Subject} - Message: {request.Message}");
        return Results.Ok(new { success = true, message = "Your message has been sent successfully." });
    }
}

public record ContactMessageRequest(string Name, string Email, string Subject, string Message);
