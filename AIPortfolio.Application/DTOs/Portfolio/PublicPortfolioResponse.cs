using AIPortfolio.Domain.Entites.Resume;
using PortfolioEntity = AIPortfolio.Domain.Entites.Portfolio.Portfolio;
using PortfolioSocialLink = AIPortfolio.Domain.Entites.Portfolio.PortfolioSocialLink;

namespace AIPortfolio.Application.DTOs.Portfolio;

public sealed record PublicPortfolioResponse(
    PortfolioEntity Profile,
    IEnumerable<ResumeEducation> Educations,
    IEnumerable<ResumeExperience> Experiences,
    IEnumerable<ResumeProject> Projects,
    IEnumerable<ResumeSkill> Skills,
    IEnumerable<ResumeCertification> Certifications,
    IEnumerable<ResumeAchievement> Achievements,
    IEnumerable<ResumeLanguage> Languages,
    IEnumerable<PortfolioSocialLink> SocialLinks,
    IEnumerable<ResumeCustomSection> CustomSections);