using AIPortfolio.Domain.Entites;
using AIPortfolio.Domain.Entites.Jobs;
using AIPortfolio.Domain.Entites.Notification;
using AIPortfolio.Domain.Entites.Portfolio;
using AIPortfolio.Domain.Entites.Resume;
using Microsoft.EntityFrameworkCore;

namespace AIPortfolio.Persistence.Data;

public sealed class AIPortfolioDbContext : DbContext
{
    public AIPortfolioDbContext(DbContextOptions<AIPortfolioDbContext> options)
        : base(options)
    {
    }

    // Identity Schema
    public DbSet<User> Users => Set<User>();
    public DbSet<UserSession> UserSessions => Set<UserSession>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    // Resume Schema
    public DbSet<Resume> Resumes => Set<Resume>();
    public DbSet<ResumeEducation> ResumeEducations => Set<ResumeEducation>();
    public DbSet<ResumeExperience> ResumeExperiences => Set<ResumeExperience>();
    public DbSet<ResumeProject> ResumeProjects => Set<ResumeProject>();
    public DbSet<ResumeSkill> ResumeSkills => Set<ResumeSkill>();
    public DbSet<ResumeCertification> ResumeCertifications => Set<ResumeCertification>();
    public DbSet<ResumeLanguage> ResumeLanguages => Set<ResumeLanguage>();
    public DbSet<ResumeCustomSection> ResumeCustomSections => Set<ResumeCustomSection>();
    public DbSet<ResumeAchievement> ResumeAchievements => Set<ResumeAchievement>();
    public DbSet<ResumeVersion> ResumeVersions => Set<ResumeVersion>();
    public DbSet<ResumeTemplate> ResumeTemplates => Set<ResumeTemplate>();

    // Portfolio Schema
    public DbSet<Portfolio> Portfolios => Set<Portfolio>();
    public DbSet<PortfolioSocialLink> PortfolioSocialLinks => Set<PortfolioSocialLink>();
    public DbSet<RecruiterMessage> RecruiterMessages => Set<RecruiterMessage>();

    // AI Schema
    public DbSet<AIChatSession> AIChatSessions => Set<AIChatSession>();
    public DbSet<AIChatMessage> AIChatMessages => Set<AIChatMessage>();
    public DbSet<AIUsage> AIUsages => Set<AIUsage>();
    public DbSet<PromptTemplate> PromptTemplates => Set<PromptTemplate>();

    // Jobs Schema
    public DbSet<JobApplication> JobApplications => Set<JobApplication>();
    public DbSet<InterviewRound> InterviewRounds => Set<InterviewRound>();

    // Notification Schema
    public DbSet<Notification> Notifications => Set<Notification>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("dbo");

        // Scan assembly and apply configurations automatically
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AIPortfolioDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}