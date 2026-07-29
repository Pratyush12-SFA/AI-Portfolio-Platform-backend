using AIPortfolio.Application.Abstractions;
using AIPortfolio.Domain.Entites.Portfolio;
using AIPortfolio.Domain.Entites.Resume;
using AIPortfolio.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace AIPortfolio.Persistence.Repositories;

internal sealed class PortfolioRepository : IPortfolioRepository
{
    private readonly AIPortfolioDbContext _context;
    private readonly IUserInfoAccessor _userInfoAccessor;

    public PortfolioRepository(
        AIPortfolioDbContext context,
        IUserInfoAccessor userInfoAccessor)
    {
        _context = context;
        _userInfoAccessor = userInfoAccessor;
    }


    // PROFILE
    public async Task<Portfolio?> GetProfileByUserIdAsync(long userId)
    {
        var portfolio = await _context.Portfolios.FirstOrDefaultAsync(p => p.UserId == userId);
        if (portfolio != null)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user != null) portfolio.FullName = user.FullName;
        }

        return portfolio;
    }

    public async Task<Portfolio?> GetProfileBySlugAsync(string slug)
    {
        var portfolio = await _context.Portfolios.FirstOrDefaultAsync(p => p.CustomSlug == slug);
        if (portfolio != null)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == portfolio.UserId);
            if (user != null) portfolio.FullName = user.FullName;
        }

        return portfolio;
    }

    public async Task UpsertProfileAsync(Portfolio portfolio)
    {
        var existing = await _context.Portfolios.FirstOrDefaultAsync(p => p.UserId == portfolio.UserId);
        if (existing == null)
        {
            if (string.IsNullOrWhiteSpace(portfolio.CustomSlug)) portfolio.CustomSlug = "user-" + portfolio.UserId;
            if (string.IsNullOrWhiteSpace(portfolio.ThemeName)) portfolio.ThemeName = "ModernDark";
            _context.Portfolios.Add(portfolio);
        }
        else
        {
            existing.ProfileHeadline = portfolio.ProfileHeadline;
            existing.ProfileSummary = portfolio.ProfileSummary;
            existing.ContactEmail = portfolio.ContactEmail;
            existing.ContactPhone = portfolio.ContactPhone;
            existing.Address = portfolio.Address;
            existing.BannerPictureUrl = portfolio.BannerPictureUrl;
            existing.ProfilePictureUrl = portfolio.ProfilePictureUrl;
            existing.SelectedThemeId = portfolio.SelectedThemeId;
            existing.SEOTitle = portfolio.SEOTitle;
            existing.SEODescription = portfolio.SEODescription;
            existing.SEOKeywords = portfolio.SEOKeywords;
            existing.IsPublic = portfolio.IsPublic;

            if (!string.IsNullOrWhiteSpace(portfolio.CustomSlug)) existing.CustomSlug = portfolio.CustomSlug;
            if (!string.IsNullOrWhiteSpace(portfolio.ThemeName)) existing.ThemeName = portfolio.ThemeName;

            existing.UpdatedBy = portfolio.CreatedBy;
            existing.UpdatedOn = DateTime.UtcNow;
            existing.UpdatedFromIp = portfolio.CreatedFromIp;
        }

        await _context.SaveChangesAsync();
    }

    // EDUCATION
    public async Task<IEnumerable<ResumeEducation>> GetEducationsByUserIdAsync(long userId)
    {
        var resumeId = await GetOrCreatePrimaryResumeIdAsync(userId);
        return await _context.ResumeEducations.Where(e => e.ResumeId == resumeId).ToListAsync();
    }

    public async Task<long> UpsertEducationAsync(ResumeEducation education)
    {
        if (education.ResumeId == 0)
            education.ResumeId = await GetOrCreatePrimaryResumeIdAsync(_userInfoAccessor.UserId);

        if (education.Id > 0)
        {
            var existing = await _context.ResumeEducations.FindAsync(education.Id);
            if (existing != null)
            {
                existing.Institution = education.Institution;
                existing.Degree = education.Degree;
                existing.FieldOfStudy = education.FieldOfStudy;
                existing.StartDate = education.StartDate;
                existing.EndDate = education.EndDate;
                existing.Grade = education.Grade;
                existing.Description = education.Description;
                existing.DisplayOrder = education.DisplayOrder;

                existing.UpdatedBy = education.CreatedBy;
                existing.UpdatedOn = DateTime.UtcNow;
                existing.UpdatedFromIp = education.CreatedFromIp;

                await _context.SaveChangesAsync();
                return existing.Id;
            }
        }

        _context.ResumeEducations.Add(education);
        await _context.SaveChangesAsync();
        return education.Id;
    }

    public async Task DeleteEducationAsync(long id, long userId)
    {
        var record = await _context.ResumeEducations.FindAsync(id);
        if (record != null)
        {
            _context.ResumeEducations.Remove(record);
            await _context.SaveChangesAsync();
        }
    }

    // EXPERIENCE
    public async Task<IEnumerable<ResumeExperience>> GetExperiencesByUserIdAsync(long userId)
    {
        var resumeId = await GetOrCreatePrimaryResumeIdAsync(userId);
        return await _context.ResumeExperiences.Where(e => e.ResumeId == resumeId).ToListAsync();
    }

    public async Task<long> UpsertExperienceAsync(ResumeExperience experience)
    {
        if (experience.ResumeId == 0)
            experience.ResumeId = await GetOrCreatePrimaryResumeIdAsync(_userInfoAccessor.UserId);

        if (experience.Id > 0)
        {
            var existing = await _context.ResumeExperiences.FindAsync(experience.Id);
            if (existing != null)
            {
                existing.CompanyName = experience.CompanyName;
                existing.Designation = experience.Designation;
                existing.EmploymentType = experience.EmploymentType;
                existing.Location = experience.Location;
                existing.StartDate = experience.StartDate;
                existing.EndDate = experience.EndDate;
                existing.Description = experience.Description;
                existing.DisplayOrder = experience.DisplayOrder;

                existing.UpdatedBy = experience.CreatedBy;
                existing.UpdatedOn = DateTime.UtcNow;
                existing.UpdatedFromIp = experience.CreatedFromIp;

                await _context.SaveChangesAsync();
                return existing.Id;
            }
        }

        _context.ResumeExperiences.Add(experience);
        await _context.SaveChangesAsync();
        return experience.Id;
    }

    public async Task DeleteExperienceAsync(long id, long userId)
    {
        var record = await _context.ResumeExperiences.FindAsync(id);
        if (record != null)
        {
            _context.ResumeExperiences.Remove(record);
            await _context.SaveChangesAsync();
        }
    }

    // PROJECT
    public async Task<IEnumerable<ResumeProject>> GetProjectsByUserIdAsync(long userId)
    {
        var resumeId = await GetOrCreatePrimaryResumeIdAsync(userId);
        return await _context.ResumeProjects.Where(p => p.ResumeId == resumeId).ToListAsync();
    }

    public async Task<long> UpsertProjectAsync(ResumeProject project)
    {
        if (project.ResumeId == 0) project.ResumeId = await GetOrCreatePrimaryResumeIdAsync(_userInfoAccessor.UserId);

        if (project.Id > 0)
        {
            var existing = await _context.ResumeProjects.FindAsync(project.Id);
            if (existing != null)
            {
                existing.Title = project.Title;
                existing.Role = project.Role;
                existing.Technologies = project.Technologies;
                existing.Url = project.Url;
                existing.StartDate = project.StartDate;
                existing.EndDate = project.EndDate;
                existing.Description = project.Description;
                existing.DisplayOrder = project.DisplayOrder;

                existing.UpdatedBy = project.CreatedBy;
                existing.UpdatedOn = DateTime.UtcNow;
                existing.UpdatedFromIp = project.CreatedFromIp;

                await _context.SaveChangesAsync();
                return existing.Id;
            }
        }

        _context.ResumeProjects.Add(project);
        await _context.SaveChangesAsync();
        return project.Id;
    }

    public async Task DeleteProjectAsync(long id, long userId)
    {
        var record = await _context.ResumeProjects.FindAsync(id);
        if (record != null)
        {
            _context.ResumeProjects.Remove(record);
            await _context.SaveChangesAsync();
        }
    }

    // SKILL
    public async Task<IEnumerable<ResumeSkill>> GetSkillsByUserIdAsync(long userId)
    {
        var resumeId = await GetOrCreatePrimaryResumeIdAsync(userId);
        return await _context.ResumeSkills.Where(s => s.ResumeId == resumeId).ToListAsync();
    }

    public async Task<long> UpsertSkillAsync(ResumeSkill skill)
    {
        if (skill.ResumeId == 0) skill.ResumeId = await GetOrCreatePrimaryResumeIdAsync(_userInfoAccessor.UserId);

        if (skill.Id > 0)
        {
            var existing = await _context.ResumeSkills.FindAsync(skill.Id);
            if (existing != null)
            {
                existing.Name = skill.Name;
                existing.ProficiencyLevel = skill.ProficiencyLevel;
                existing.Category = skill.Category;
                existing.DisplayOrder = skill.DisplayOrder;

                existing.UpdatedBy = skill.CreatedBy;
                existing.UpdatedOn = DateTime.UtcNow;
                existing.UpdatedFromIp = skill.CreatedFromIp;

                await _context.SaveChangesAsync();
                return existing.Id;
            }
        }

        _context.ResumeSkills.Add(skill);
        await _context.SaveChangesAsync();
        return skill.Id;
    }

    public async Task DeleteSkillAsync(long id, long userId)
    {
        var record = await _context.ResumeSkills.FindAsync(id);
        if (record != null)
        {
            _context.ResumeSkills.Remove(record);
            await _context.SaveChangesAsync();
        }
    }

    // CERTIFICATION
    public async Task<IEnumerable<ResumeCertification>> GetCertificationsByUserIdAsync(long userId)
    {
        var resumeId = await GetOrCreatePrimaryResumeIdAsync(userId);
        return await _context.ResumeCertifications.Where(c => c.ResumeId == resumeId).ToListAsync();
    }

    public async Task<long> UpsertCertificationAsync(ResumeCertification certification)
    {
        if (certification.ResumeId == 0)
            certification.ResumeId = await GetOrCreatePrimaryResumeIdAsync(_userInfoAccessor.UserId);

        if (certification.Id > 0)
        {
            var existing = await _context.ResumeCertifications.FindAsync(certification.Id);
            if (existing != null)
            {
                existing.Name = certification.Name;
                existing.IssuingOrganization = certification.IssuingOrganization;
                existing.IssueDate = certification.IssueDate;
                existing.ExpirationDate = certification.ExpirationDate;
                existing.CredentialId = certification.CredentialId;
                existing.CredentialUrl = certification.CredentialUrl;
                existing.DisplayOrder = certification.DisplayOrder;

                existing.UpdatedBy = certification.CreatedBy;
                existing.UpdatedOn = DateTime.UtcNow;
                existing.UpdatedFromIp = certification.CreatedFromIp;

                await _context.SaveChangesAsync();
                return existing.Id;
            }
        }

        _context.ResumeCertifications.Add(certification);
        await _context.SaveChangesAsync();
        return certification.Id;
    }

    public async Task DeleteCertificationAsync(long id, long userId)
    {
        var record = await _context.ResumeCertifications.FindAsync(id);
        if (record != null)
        {
            _context.ResumeCertifications.Remove(record);
            await _context.SaveChangesAsync();
        }
    }

    // ACHIEVEMENT
    public async Task<IEnumerable<ResumeAchievement>> GetAchievementsByUserIdAsync(long userId)
    {
        var resumeId = await GetOrCreatePrimaryResumeIdAsync(userId);
        return await _context.ResumeAchievements.Where(a => a.ResumeId == resumeId).ToListAsync();
    }

    public async Task<long> UpsertAchievementAsync(ResumeAchievement achievement)
    {
        if (achievement.ResumeId == 0)
            achievement.ResumeId = await GetOrCreatePrimaryResumeIdAsync(_userInfoAccessor.UserId);

        if (achievement.Id > 0)
        {
            var existing = await _context.ResumeAchievements.FindAsync(achievement.Id);
            if (existing != null)
            {
                existing.Title = achievement.Title;
                existing.Description = achievement.Description;
                existing.DisplayOrder = achievement.DisplayOrder;

                existing.UpdatedBy = achievement.CreatedBy;
                existing.UpdatedOn = DateTime.UtcNow;
                existing.UpdatedFromIp = achievement.CreatedFromIp;

                await _context.SaveChangesAsync();
                return existing.Id;
            }
        }

        _context.ResumeAchievements.Add(achievement);
        await _context.SaveChangesAsync();
        return achievement.Id;
    }

    public async Task DeleteAchievementAsync(long id, long userId)
    {
        var record = await _context.ResumeAchievements.FindAsync(id);
        if (record != null)
        {
            _context.ResumeAchievements.Remove(record);
            await _context.SaveChangesAsync();
        }
    }

    // LANGUAGE
    public async Task<IEnumerable<ResumeLanguage>> GetLanguagesByUserIdAsync(long userId)
    {
        var resumeId = await GetOrCreatePrimaryResumeIdAsync(userId);
        return await _context.ResumeLanguages.Where(l => l.ResumeId == resumeId).ToListAsync();
    }

    public async Task<long> UpsertLanguageAsync(ResumeLanguage language)
    {
        if (language.ResumeId == 0) language.ResumeId = await GetOrCreatePrimaryResumeIdAsync(_userInfoAccessor.UserId);

        if (language.Id > 0)
        {
            var existing = await _context.ResumeLanguages.FindAsync(language.Id);
            if (existing != null)
            {
                existing.Name = language.Name;
                existing.ProficiencyLevel = language.ProficiencyLevel;
                existing.DisplayOrder = language.DisplayOrder;

                existing.UpdatedBy = language.CreatedBy;
                existing.UpdatedOn = DateTime.UtcNow;
                existing.UpdatedFromIp = language.CreatedFromIp;

                await _context.SaveChangesAsync();
                return existing.Id;
            }
        }

        _context.ResumeLanguages.Add(language);
        await _context.SaveChangesAsync();
        return language.Id;
    }

    public async Task DeleteLanguageAsync(long id, long userId)
    {
        var record = await _context.ResumeLanguages.FindAsync(id);
        if (record != null)
        {
            _context.ResumeLanguages.Remove(record);
            await _context.SaveChangesAsync();
        }
    }

    // SOCIAL LINK
    public async Task<IEnumerable<PortfolioSocialLink>> GetSocialLinksByUserIdAsync(long userId)
    {
        var portfolio = await _context.Portfolios.FirstOrDefaultAsync(p => p.UserId == userId);
        if (portfolio == null) return Enumerable.Empty<PortfolioSocialLink>();
        return await _context.PortfolioSocialLinks.Where(sl => sl.PortfolioId == portfolio.Id).ToListAsync();
    }

    public async Task<long> UpsertSocialLinkAsync(PortfolioSocialLink socialLink)
    {
        if (socialLink.PortfolioId == 0)
        {
            var portfolio = await _context.Portfolios.FirstOrDefaultAsync(p => p.UserId == _userInfoAccessor.UserId);
            if (portfolio == null)
            {
                portfolio = new Portfolio
                {
                    UserId = _userInfoAccessor.UserId,
                    CustomSlug = "user-" + _userInfoAccessor.UserId,
                    CreatedBy = socialLink.CreatedBy,
                    CreatedFromIp = socialLink.CreatedFromIp,
                    RowVersion = new byte[8]
                };
                _context.Portfolios.Add(portfolio);
                await _context.SaveChangesAsync();
            }

            socialLink.PortfolioId = portfolio.Id;
        }

        if (socialLink.Id > 0)
        {
            var existing = await _context.PortfolioSocialLinks.FindAsync(socialLink.Id);
            if (existing != null)
            {
                existing.PlatformName = socialLink.PlatformName;
                existing.Url = socialLink.Url;
                existing.DisplayOrder = socialLink.DisplayOrder;

                existing.UpdatedBy = socialLink.CreatedBy;
                existing.UpdatedOn = DateTime.UtcNow;
                existing.UpdatedFromIp = socialLink.CreatedFromIp;

                await _context.SaveChangesAsync();
                return existing.Id;
            }
        }

        _context.PortfolioSocialLinks.Add(socialLink);
        await _context.SaveChangesAsync();
        return socialLink.Id;
    }

    public async Task DeleteSocialLinkAsync(long id, long userId)
    {
        var record = await _context.PortfolioSocialLinks.FindAsync(id);
        if (record != null)
        {
            _context.PortfolioSocialLinks.Remove(record);
            await _context.SaveChangesAsync();
        }
    }

    // CUSTOM SECTION
    public async Task<IEnumerable<ResumeCustomSection>> GetCustomSectionsByUserIdAsync(long userId)
    {
        var resumeId = await GetOrCreatePrimaryResumeIdAsync(userId);
        return await _context.ResumeCustomSections.Where(cs => cs.ResumeId == resumeId).ToListAsync();
    }

    public async Task<long> UpsertCustomSectionAsync(ResumeCustomSection customSection)
    {
        if (customSection.ResumeId == 0)
            customSection.ResumeId = await GetOrCreatePrimaryResumeIdAsync(_userInfoAccessor.UserId);

        if (customSection.Id > 0)
        {
            var existing = await _context.ResumeCustomSections.FindAsync(customSection.Id);
            if (existing != null)
            {
                existing.SectionName = customSection.SectionName;
                existing.Content = customSection.Content;
                existing.DisplayOrder = customSection.DisplayOrder;

                existing.UpdatedBy = customSection.CreatedBy;
                existing.UpdatedOn = DateTime.UtcNow;
                existing.UpdatedFromIp = customSection.CreatedFromIp;

                await _context.SaveChangesAsync();
                return existing.Id;
            }
        }

        _context.ResumeCustomSections.Add(customSection);
        await _context.SaveChangesAsync();
        return customSection.Id;
    }

    public async Task DeleteCustomSectionAsync(long id, long userId)
    {
        var record = await _context.ResumeCustomSections.FindAsync(id);
        if (record != null)
        {
            _context.ResumeCustomSections.Remove(record);
            await _context.SaveChangesAsync();
        }
    }

    private async Task<long> GetOrCreatePrimaryResumeIdAsync(long userId)
    {
        var resume = await _context.Resumes.FirstOrDefaultAsync(r => r.UserId == userId && r.IsPrimary);
        if (resume == null)
        {
            resume = new Resume
            {
                UserId = userId,
                Title = "Primary Resume",
                IsPrimary = true
            };
            _context.Resumes.Add(resume);
            await _context.SaveChangesAsync();
        }

        return resume.Id;
    }
}