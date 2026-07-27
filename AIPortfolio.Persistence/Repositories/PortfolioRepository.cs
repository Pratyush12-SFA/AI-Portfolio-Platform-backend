using System.Data;
using AIPortfolio.Application.Abstractions;
using AIPortfolio.Domain.Entites;
using AIPortfolio.Persistence.Connections;
using Dapper;

namespace AIPortfolio.Persistence.Repositories;

internal sealed class PortfolioRepository : IPortfolioRepository
{
    private readonly DapperContext _context;

    public PortfolioRepository(DapperContext context)
    {
        _context = context;
    }

    // PROFILE
    public async Task<Profile?> GetProfileByUserIdAsync(long userId)
    {
        using IDbConnection connection = _context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<Profile>(
            "Portfolio.usp_Profile_GetByUserId",
            new { UserId = userId },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<Profile?> GetProfileBySlugAsync(string slug)
    {
        using IDbConnection connection = _context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<Profile>(
            "Portfolio.usp_Profile_GetBySlug",
            new { CustomSlug = slug },
            commandType: CommandType.StoredProcedure);
    }

    public async Task UpsertProfileAsync(Profile profile)
    {
        using IDbConnection connection = _context.CreateConnection();
        await connection.ExecuteAsync(
            "Portfolio.usp_Profile_Upsert",
            new
            {
                profile.UserId,
                profile.Headline,
                profile.Summary,
                profile.PhoneNumber,
                profile.ContactEmail,
                profile.Address,
                profile.ThemeName,
                profile.CustomSlug,
                profile.IsDarkModePreferred,
                profile.CreatedBy,
                profile.CreatedFromIp
            },
            commandType: CommandType.StoredProcedure);
    }

    // EDUCATION
    public async Task<IEnumerable<Education>> GetEducationsByUserIdAsync(long userId)
    {
        using IDbConnection connection = _context.CreateConnection();
        return await connection.QueryAsync<Education>(
            "Portfolio.usp_Education_GetByUserId",
            new { UserId = userId },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<long> UpsertEducationAsync(Education education)
    {
        using IDbConnection connection = _context.CreateConnection();
        return await connection.QuerySingleAsync<long>(
            "Portfolio.usp_Education_Upsert",
            new
            {
                education.Id,
                education.UserId,
                education.Institution,
                education.Degree,
                education.FieldOfStudy,
                education.StartDate,
                education.EndDate,
                education.Grade,
                education.Description,
                education.DisplayOrder,
                education.CreatedBy,
                education.CreatedFromIp
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task DeleteEducationAsync(long id, long userId)
    {
        using IDbConnection connection = _context.CreateConnection();
        await connection.ExecuteAsync(
            "Portfolio.usp_Education_Delete",
            new { Id = id, UserId = userId },
            commandType: CommandType.StoredProcedure);
    }

    // EXPERIENCE
    public async Task<IEnumerable<Experience>> GetExperiencesByUserIdAsync(long userId)
    {
        using IDbConnection connection = _context.CreateConnection();
        return await connection.QueryAsync<Experience>(
            "Portfolio.usp_Experience_GetByUserId",
            new { UserId = userId },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<long> UpsertExperienceAsync(Experience experience)
    {
        using IDbConnection connection = _context.CreateConnection();
        return await connection.QuerySingleAsync<long>(
            "Portfolio.usp_Experience_Upsert",
            new
            {
                experience.Id,
                experience.UserId,
                experience.CompanyName,
                experience.Designation,
                experience.EmploymentType,
                experience.Location,
                experience.StartDate,
                experience.EndDate,
                experience.Description,
                experience.DisplayOrder,
                experience.CreatedBy,
                experience.CreatedFromIp
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task DeleteExperienceAsync(long id, long userId)
    {
        using IDbConnection connection = _context.CreateConnection();
        await connection.ExecuteAsync(
            "Portfolio.usp_Experience_Delete",
            new { Id = id, UserId = userId },
            commandType: CommandType.StoredProcedure);
    }

    // PROJECT
    public async Task<IEnumerable<Project>> GetProjectsByUserIdAsync(long userId)
    {
        using IDbConnection connection = _context.CreateConnection();
        return await connection.QueryAsync<Project>(
            "Portfolio.usp_Project_GetByUserId",
            new { UserId = userId },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<long> UpsertProjectAsync(Project project)
    {
        using IDbConnection connection = _context.CreateConnection();
        return await connection.QuerySingleAsync<long>(
            "Portfolio.usp_Project_Upsert",
            new
            {
                project.Id,
                project.UserId,
                project.Title,
                project.ShortDescription,
                project.Description,
                project.TechStack,
                project.GithubUrl,
                project.LiveDemoUrl,
                project.ThumbnailUrl,
                project.DisplayOrder,
                project.IsFeatured,
                project.CreatedBy,
                project.CreatedFromIp
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task DeleteProjectAsync(long id, long userId)
    {
        using IDbConnection connection = _context.CreateConnection();
        await connection.ExecuteAsync(
            "Portfolio.usp_Project_Delete",
            new { Id = id, UserId = userId },
            commandType: CommandType.StoredProcedure);
    }

    // SKILL
    public async Task<IEnumerable<Skill>> GetSkillsByUserIdAsync(long userId)
    {
        using IDbConnection connection = _context.CreateConnection();
        return await connection.QueryAsync<Skill>(
            "Portfolio.usp_Skill_GetByUserId",
            new { UserId = userId },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<long> UpsertSkillAsync(Skill skill)
    {
        using IDbConnection connection = _context.CreateConnection();
        return await connection.QuerySingleAsync<long>(
            "Portfolio.usp_Skill_Upsert",
            new
            {
                skill.Id,
                skill.UserId,
                skill.Name,
                skill.Category,
                skill.ProficiencyPercentage,
                skill.DisplayOrder,
                skill.CreatedBy,
                skill.CreatedFromIp
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task DeleteSkillAsync(long id, long userId)
    {
        using IDbConnection connection = _context.CreateConnection();
        await connection.ExecuteAsync(
            "Portfolio.usp_Skill_Delete",
            new { Id = id, UserId = userId },
            commandType: CommandType.StoredProcedure);
    }

    // CERTIFICATION
    public async Task<IEnumerable<Certification>> GetCertificationsByUserIdAsync(long userId)
    {
        using IDbConnection connection = _context.CreateConnection();
        return await connection.QueryAsync<Certification>(
            "Portfolio.usp_Certification_GetByUserId",
            new { UserId = userId },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<long> UpsertCertificationAsync(Certification certification)
    {
        using IDbConnection connection = _context.CreateConnection();
        return await connection.QuerySingleAsync<long>(
            "Portfolio.usp_Certification_Upsert",
            new
            {
                certification.Id,
                certification.UserId,
                certification.Title,
                certification.IssuingOrganization,
                certification.CertificateUrl,
                certification.IssueDate,
                certification.ExpiryDate,
                certification.DisplayOrder,
                certification.CreatedBy,
                certification.CreatedFromIp
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task DeleteCertificationAsync(long id, long userId)
    {
        using IDbConnection connection = _context.CreateConnection();
        await connection.ExecuteAsync(
            "Portfolio.usp_Certification_Delete",
            new { Id = id, UserId = userId },
            commandType: CommandType.StoredProcedure);
    }

    // ACHIEVEMENT
    public async Task<IEnumerable<Achievement>> GetAchievementsByUserIdAsync(long userId)
    {
        using IDbConnection connection = _context.CreateConnection();
        return await connection.QueryAsync<Achievement>(
            "Portfolio.usp_Achievement_GetByUserId",
            new { UserId = userId },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<long> UpsertAchievementAsync(Achievement achievement)
    {
        using IDbConnection connection = _context.CreateConnection();
        return await connection.QuerySingleAsync<long>(
            "Portfolio.usp_Achievement_Upsert",
            new
            {
                achievement.Id,
                achievement.UserId,
                achievement.Title,
                achievement.Issuer,
                achievement.DateReceived,
                achievement.Description,
                achievement.DisplayOrder,
                achievement.CreatedBy,
                achievement.CreatedFromIp
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task DeleteAchievementAsync(long id, long userId)
    {
        using IDbConnection connection = _context.CreateConnection();
        await connection.ExecuteAsync(
            "Portfolio.usp_Achievement_Delete",
            new { Id = id, UserId = userId },
            commandType: CommandType.StoredProcedure);
    }

    // LANGUAGE
    public async Task<IEnumerable<Language>> GetLanguagesByUserIdAsync(long userId)
    {
        using IDbConnection connection = _context.CreateConnection();
        return await connection.QueryAsync<Language>(
            "Portfolio.usp_Language_GetByUserId",
            new { UserId = userId },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<long> UpsertLanguageAsync(Language language)
    {
        using IDbConnection connection = _context.CreateConnection();
        return await connection.QuerySingleAsync<long>(
            "Portfolio.usp_Language_Upsert",
            new
            {
                language.Id,
                language.UserId,
                language.LanguageName,
                language.Proficiency,
                language.DisplayOrder,
                language.CreatedBy,
                language.CreatedFromIp
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task DeleteLanguageAsync(long id, long userId)
    {
        using IDbConnection connection = _context.CreateConnection();
        await connection.ExecuteAsync(
            "Portfolio.usp_Language_Delete",
            new { Id = id, UserId = userId },
            commandType: CommandType.StoredProcedure);
    }

    // SOCIAL LINK
    public async Task<IEnumerable<SocialLink>> GetSocialLinksByUserIdAsync(long userId)
    {
        using IDbConnection connection = _context.CreateConnection();
        return await connection.QueryAsync<SocialLink>(
            "Portfolio.usp_SocialLink_GetByUserId",
            new { UserId = userId },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<long> UpsertSocialLinkAsync(SocialLink socialLink)
    {
        using IDbConnection connection = _context.CreateConnection();
        return await connection.QuerySingleAsync<long>(
            "Portfolio.usp_SocialLink_Upsert",
            new
            {
                socialLink.Id,
                socialLink.UserId,
                socialLink.PlatformName,
                socialLink.Url,
                socialLink.CreatedBy,
                socialLink.CreatedFromIp
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task DeleteSocialLinkAsync(long id, long userId)
    {
        using IDbConnection connection = _context.CreateConnection();
        await connection.ExecuteAsync(
            "Portfolio.usp_SocialLink_Delete",
            new { Id = id, UserId = userId },
            commandType: CommandType.StoredProcedure);
    }

    // CUSTOM SECTION
    public async Task<IEnumerable<CustomSection>> GetCustomSectionsByUserIdAsync(long userId)
    {
        using IDbConnection connection = _context.CreateConnection();
        return await connection.QueryAsync<CustomSection>(
            "Portfolio.usp_CustomSection_GetByUserId",
            new { UserId = userId },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<long> UpsertCustomSectionAsync(CustomSection customSection)
    {
        using IDbConnection connection = _context.CreateConnection();
        return await connection.QuerySingleAsync<long>(
            "Portfolio.usp_CustomSection_Upsert",
            new
            {
                customSection.Id,
                customSection.UserId,
                customSection.SectionTitle,
                customSection.Content,
                customSection.DisplayOrder,
                customSection.CreatedBy,
                customSection.CreatedFromIp
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task DeleteCustomSectionAsync(long id, long userId)
    {
        using IDbConnection connection = _context.CreateConnection();
        await connection.ExecuteAsync(
            "Portfolio.usp_CustomSection_Delete",
            new { Id = id, UserId = userId },
            commandType: CommandType.StoredProcedure);
    }
}
