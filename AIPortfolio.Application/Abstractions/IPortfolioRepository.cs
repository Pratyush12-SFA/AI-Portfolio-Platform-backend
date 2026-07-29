using AIPortfolio.Domain.Entites.Portfolio;
using AIPortfolio.Domain.Entites.Resume;

namespace AIPortfolio.Application.Abstractions;

public interface IPortfolioRepository
{
    // Profile
    Task<Portfolio?> GetProfileByUserIdAsync(long userId);
    Task<Portfolio?> GetProfileBySlugAsync(string slug);
    Task UpsertProfileAsync(Portfolio portfolio);

    // Education
    Task<IEnumerable<ResumeEducation>> GetEducationsByUserIdAsync(long userId);
    Task<long> UpsertEducationAsync(ResumeEducation education);
    Task DeleteEducationAsync(long id, long userId);

    // Experience
    Task<IEnumerable<ResumeExperience>> GetExperiencesByUserIdAsync(long userId);
    Task<long> UpsertExperienceAsync(ResumeExperience experience);
    Task DeleteExperienceAsync(long id, long userId);

    // Project
    Task<IEnumerable<ResumeProject>> GetProjectsByUserIdAsync(long userId);
    Task<long> UpsertProjectAsync(ResumeProject project);
    Task DeleteProjectAsync(long id, long userId);

    // Skill
    Task<IEnumerable<ResumeSkill>> GetSkillsByUserIdAsync(long userId);
    Task<long> UpsertSkillAsync(ResumeSkill skill);
    Task DeleteSkillAsync(long id, long userId);

    // Certification
    Task<IEnumerable<ResumeCertification>> GetCertificationsByUserIdAsync(long userId);
    Task<long> UpsertCertificationAsync(ResumeCertification certification);
    Task DeleteCertificationAsync(long id, long userId);

    // Achievement
    Task<IEnumerable<ResumeAchievement>> GetAchievementsByUserIdAsync(long userId);
    Task<long> UpsertAchievementAsync(ResumeAchievement achievement);
    Task DeleteAchievementAsync(long id, long userId);

    // Language
    Task<IEnumerable<ResumeLanguage>> GetLanguagesByUserIdAsync(long userId);
    Task<long> UpsertLanguageAsync(ResumeLanguage language);
    Task DeleteLanguageAsync(long id, long userId);

    // SocialLink
    Task<IEnumerable<PortfolioSocialLink>> GetSocialLinksByUserIdAsync(long userId);
    Task<long> UpsertSocialLinkAsync(PortfolioSocialLink socialLink);
    Task DeleteSocialLinkAsync(long id, long userId);

    // CustomSection
    Task<IEnumerable<ResumeCustomSection>> GetCustomSectionsByUserIdAsync(long userId);
    Task<long> UpsertCustomSectionAsync(ResumeCustomSection customSection);
    Task DeleteCustomSectionAsync(long id, long userId);
}