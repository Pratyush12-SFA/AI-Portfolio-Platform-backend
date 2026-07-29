using AIPortfolio.Domain.Common;

namespace AIPortfolio.Domain.Entites.Resume;

public sealed class Resume : BaseEntity
{
    public long UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsPrimary { get; set; }
    public string? TargetJobTitle { get; set; }
    public string? Summary { get; set; }

    // Navigation
    public ICollection<ResumeEducation> Educations { get; set; } = [];
    public ICollection<ResumeExperience> Experiences { get; set; } = [];
    public ICollection<ResumeProject> Projects { get; set; } = [];
    public ICollection<ResumeSkill> Skills { get; set; } = [];
    public ICollection<ResumeCertification> Certifications { get; set; } = [];
    public ICollection<ResumeLanguage> Languages { get; set; } = [];
    public ICollection<ResumeCustomSection> CustomSections { get; set; } = [];
    public ICollection<ResumeAchievement> Achievements { get; set; } = [];
    public ICollection<ResumeVersion> Versions { get; set; } = [];
}