using AIPortfolio.Application.Abstractions;
using AIPortfolio.Domain.Entites;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            profile = new Profile
            {
                UserId = userInfoAccessor.UserId,
                ThemeName = "ModernDark",
                IsDarkModePreferred = true
            };
        }
        return Results.Ok(profile);
    }

    public static async Task<IResult> UpsertProfile(
        Profile profileRequest,
        IPortfolioRepository repository,
        IUserInfoAccessor userInfoAccessor)
    {
        if (!userInfoAccessor.IsAuthenticated) return Results.Unauthorized();

        profileRequest.UserId = userInfoAccessor.UserId;
        profileRequest.CreatedBy = userInfoAccessor.Email;
        profileRequest.CreatedFromIp = userInfoAccessor.GetRemoteIp();

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
        Education education,
        IPortfolioRepository repository,
        IUserInfoAccessor userInfoAccessor)
    {
        if (!userInfoAccessor.IsAuthenticated) return Results.Unauthorized();

        education.UserId = userInfoAccessor.UserId;
        education.CreatedBy = userInfoAccessor.Email;
        education.CreatedFromIp = userInfoAccessor.GetRemoteIp();

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
        Experience experience,
        IPortfolioRepository repository,
        IUserInfoAccessor userInfoAccessor)
    {
        if (!userInfoAccessor.IsAuthenticated) return Results.Unauthorized();

        experience.UserId = userInfoAccessor.UserId;
        experience.CreatedBy = userInfoAccessor.Email;
        experience.CreatedFromIp = userInfoAccessor.GetRemoteIp();

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
        Project project,
        IPortfolioRepository repository,
        IUserInfoAccessor userInfoAccessor)
    {
        if (!userInfoAccessor.IsAuthenticated) return Results.Unauthorized();

        project.UserId = userInfoAccessor.UserId;
        project.CreatedBy = userInfoAccessor.Email;
        project.CreatedFromIp = userInfoAccessor.GetRemoteIp();

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
        Skill skill,
        IPortfolioRepository repository,
        IUserInfoAccessor userInfoAccessor)
    {
        if (!userInfoAccessor.IsAuthenticated) return Results.Unauthorized();

        skill.UserId = userInfoAccessor.UserId;
        skill.CreatedBy = userInfoAccessor.Email;
        skill.CreatedFromIp = userInfoAccessor.GetRemoteIp();

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
        Certification certification,
        IPortfolioRepository repository,
        IUserInfoAccessor userInfoAccessor)
    {
        if (!userInfoAccessor.IsAuthenticated) return Results.Unauthorized();

        certification.UserId = userInfoAccessor.UserId;
        certification.CreatedBy = userInfoAccessor.Email;
        certification.CreatedFromIp = userInfoAccessor.GetRemoteIp();

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
        Achievement achievement,
        IPortfolioRepository repository,
        IUserInfoAccessor userInfoAccessor)
    {
        if (!userInfoAccessor.IsAuthenticated) return Results.Unauthorized();

        achievement.UserId = userInfoAccessor.UserId;
        achievement.CreatedBy = userInfoAccessor.Email;
        achievement.CreatedFromIp = userInfoAccessor.GetRemoteIp();

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
        Language language,
        IPortfolioRepository repository,
        IUserInfoAccessor userInfoAccessor)
    {
        if (!userInfoAccessor.IsAuthenticated) return Results.Unauthorized();

        language.UserId = userInfoAccessor.UserId;
        language.CreatedBy = userInfoAccessor.Email;
        language.CreatedFromIp = userInfoAccessor.GetRemoteIp();

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
        SocialLink socialLink,
        IPortfolioRepository repository,
        IUserInfoAccessor userInfoAccessor)
    {
        if (!userInfoAccessor.IsAuthenticated) return Results.Unauthorized();

        socialLink.UserId = userInfoAccessor.UserId;
        socialLink.CreatedBy = userInfoAccessor.Email;
        socialLink.CreatedFromIp = userInfoAccessor.GetRemoteIp();

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
        CustomSection customSection,
        IPortfolioRepository repository,
        IUserInfoAccessor userInfoAccessor)
    {
        if (!userInfoAccessor.IsAuthenticated) return Results.Unauthorized();

        customSection.UserId = userInfoAccessor.UserId;
        customSection.CreatedBy = userInfoAccessor.Email;
        customSection.CreatedFromIp = userInfoAccessor.GetRemoteIp();

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
