using AIPortfolio.Domain.Entites;

namespace AIPortfolio.Application.Abstractions;

public interface IPortfolioRepository
{
    // Profile
    Task<Profile?> GetProfileByUserIdAsync(long userId);
    Task<Profile?> GetProfileBySlugAsync(string slug);
    Task UpsertProfileAsync(Profile profile);

    // Education
    Task<IEnumerable<Education>> GetEducationsByUserIdAsync(long userId);
    Task<long> UpsertEducationAsync(Education education);
    Task DeleteEducationAsync(long id, long userId);

    // Experience
    Task<IEnumerable<Experience>> GetExperiencesByUserIdAsync(long userId);
    Task<long> UpsertExperienceAsync(Experience experience);
    Task DeleteExperienceAsync(long id, long userId);

    // Project
    Task<IEnumerable<Project>> GetProjectsByUserIdAsync(long userId);
    Task<long> UpsertProjectAsync(Project project);
    Task DeleteProjectAsync(long id, long userId);

    // Skill
    Task<IEnumerable<Skill>> GetSkillsByUserIdAsync(long userId);
    Task<long> UpsertSkillAsync(Skill skill);
    Task DeleteSkillAsync(long id, long userId);

    // Certification
    Task<IEnumerable<Certification>> GetCertificationsByUserIdAsync(long userId);
    Task<long> UpsertCertificationAsync(Certification certification);
    Task DeleteCertificationAsync(long id, long userId);

    // Achievement
    Task<IEnumerable<Achievement>> GetAchievementsByUserIdAsync(long userId);
    Task<long> UpsertAchievementAsync(Achievement achievement);
    Task DeleteAchievementAsync(long id, long userId);

    // Language
    Task<IEnumerable<Language>> GetLanguagesByUserIdAsync(long userId);
    Task<long> UpsertLanguageAsync(Language language);
    Task DeleteLanguageAsync(long id, long userId);

    // SocialLink
    Task<IEnumerable<SocialLink>> GetSocialLinksByUserIdAsync(long userId);
    Task<long> UpsertSocialLinkAsync(SocialLink socialLink);
    Task DeleteSocialLinkAsync(long id, long userId);

    // CustomSection
    Task<IEnumerable<CustomSection>> GetCustomSectionsByUserIdAsync(long userId);
    Task<long> UpsertCustomSectionAsync(CustomSection customSection);
    Task DeleteCustomSectionAsync(long id, long userId);
}
