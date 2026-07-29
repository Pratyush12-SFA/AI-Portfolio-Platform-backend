using AIPortfolio.Domain.Entites;
using AIPortfolio.Domain.Entites.Jobs;
using NotificationEntity = AIPortfolio.Domain.Entites.Notification.Notification;
using AIPortfolio.Domain.Entites.Portfolio;
using AIPortfolio.Domain.Entites.Resume;
using AIPortfolio.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AIPortfolio.Persistence.Seeding;

public static class DevelopmentSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AIPortfolioDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("DevelopmentSeeder");

        try
        {
            await context.Database.EnsureCreatedAsync();

            if (await context.Users.AnyAsync())
            {
                logger.LogInformation("Database already contains user data. Skipping seed.");
                return;
            }

            logger.LogInformation("Seeding development data for Ascend Career OS...");

            // 1. Seed Demo User
            var demoUser = new User
            {
                FullName = "Pratyush Sharma",
                Email = "pratyush@ascend.ai",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!"),
                IsActive = true,
                IsEmailVerified = true,
                ProfilePictureUrl = "https://images.unsplash.com/photo-1534528741775-53994a69daeb?w=400",
                CreatedBy = "system_seeder",
                CreatedFromIp = "127.0.0.1",
                CreatedOn = DateTime.UtcNow
            };

            context.Users.Add(demoUser);
            await context.SaveChangesAsync();

            // 2. Seed Primary Resume
            var primaryResume = new Resume
            {
                UserId = demoUser.Id,
                Title = "Senior Full Stack & AI Architect",
                IsPrimary = true,
                TargetJobTitle = "Staff Full Stack AI Engineer",
                Summary = "Principal engineer specializing in distributed cloud platforms, .NET 10 Minimal APIs, React 19, and generative AI workflow agents.",
                CreatedBy = "system_seeder",
                CreatedFromIp = "127.0.0.1",
                CreatedOn = DateTime.UtcNow
            };

            context.Resumes.Add(primaryResume);
            await context.SaveChangesAsync();

            // 3. Seed Resume Child Elements
            context.ResumeEducations.AddRange(
                new ResumeEducation
                {
                    ResumeId = primaryResume.Id,
                    Institution = "Stanford University",
                    Degree = "Master of Science",
                    FieldOfStudy = "Computer Science (AI Specialization)",
                    StartDate = new DateTime(2018, 9, 1),
                    EndDate = new DateTime(2020, 6, 1),
                    Grade = "3.9 / 4.0",
                    Description = "Focused on deep learning models and distributed computer systems.",
                    OrderIndex = 1,
                    CreatedBy = "system_seeder",
                    CreatedFromIp = "127.0.0.1"
                }
            );

            context.ResumeExperiences.AddRange(
                new ResumeExperience
                {
                    ResumeId = primaryResume.Id,
                    CompanyName = "Ascend AI Labs",
                    JobTitle = "Staff AI Software Architect",
                    Location = "San Francisco, CA (Hybrid)",
                    StartDate = new DateTime(2022, 3, 1),
                    IsCurrent = true,
                    Description = "Architected high-throughput AI agent pipelines using .NET 10, MediatR, and Gemini 2.5 Flash API.",
                    Responsibilities = "• Built real-time WebSocket streaming for interactive AI resume feedback.\n• Optimized SQL query performance by 45% using EF Core code-first indexing strategy.\n• Supervised frontend redesign using Tailwind v4 and React Query v5.",
                    OrderIndex = 1,
                    CreatedBy = "system_seeder",
                    CreatedFromIp = "127.0.0.1"
                },
                new ResumeExperience
                {
                    ResumeId = primaryResume.Id,
                    CompanyName = "Vercel Inc.",
                    JobTitle = "Senior Frontend Engineer",
                    Location = "Remote",
                    StartDate = new DateTime(2020, 7, 1),
                    EndDate = new DateTime(2022, 2, 28),
                    IsCurrent = false,
                    Description = "Engineered core dashboard features and optimized Core Web Vitals across major product lines.",
                    OrderIndex = 2,
                    CreatedBy = "system_seeder",
                    CreatedFromIp = "127.0.0.1"
                }
            );

            context.ResumeProjects.AddRange(
                new ResumeProject
                {
                    ResumeId = primaryResume.Id,
                    Title = "Ascend - AI Career Operating System",
                    Description = "An enterprise-grade SaaS platform enabling job seekers to optimize resumes, track applications, and simulate technical interviews.",
                    TechStack = ".NET 10, EF Core, React 19, Tailwind CSS, TypeScript, SqlServer",
                    ProjectUrl = "https://ascend-career.app",
                    GithubUrl = "https://github.com/Pratyush12-SFA/ai-portfolio-platform",
                    OrderIndex = 1,
                    CreatedBy = "system_seeder",
                    CreatedFromIp = "127.0.0.1"
                }
            );

            context.ResumeSkills.AddRange(
                new ResumeSkill { ResumeId = primaryResume.Id, Name = "C# / .NET 10", Category = "Backend", ProficiencyLevel = "Expert", OrderIndex = 1, CreatedBy = "system_seeder", CreatedFromIp = "127.0.0.1" },
                new ResumeSkill { ResumeId = primaryResume.Id, Name = "React 19 / TypeScript", Category = "Frontend", ProficiencyLevel = "Expert", OrderIndex = 2, CreatedBy = "system_seeder", CreatedFromIp = "127.0.0.1" },
                new ResumeSkill { ResumeId = primaryResume.Id, Name = "SQL Server & EF Core", Category = "Database", ProficiencyLevel = "Advanced", OrderIndex = 3, CreatedBy = "system_seeder", CreatedFromIp = "127.0.0.1" },
                new ResumeSkill { ResumeId = primaryResume.Id, Name = "Generative AI / LLM Orchestration", Category = "AI", ProficiencyLevel = "Advanced", OrderIndex = 4, CreatedBy = "system_seeder", CreatedFromIp = "127.0.0.1" }
            );

            context.ResumeCertifications.AddRange(
                new ResumeCertification
                {
                    ResumeId = primaryResume.Id,
                    Name = "AWS Certified Solutions Architect - Professional",
                    IssuingOrganization = "Amazon Web Services",
                    IssueDate = new DateTime(2023, 5, 15),
                    CredentialId = "AWS-PSA-990812",
                    CredentialUrl = "https://aws.amazon.com/verification",
                    OrderIndex = 1,
                    CreatedBy = "system_seeder",
                    CreatedFromIp = "127.0.0.1"
                }
            );

            context.ResumeLanguages.AddRange(
                new ResumeLanguage { ResumeId = primaryResume.Id, Name = "English", ProficiencyLevel = "Native / Fluent", OrderIndex = 1, CreatedBy = "system_seeder", CreatedFromIp = "127.0.0.1" },
                new ResumeLanguage { ResumeId = primaryResume.Id, Name = "Hindi", ProficiencyLevel = "Native", OrderIndex = 2, CreatedBy = "system_seeder", CreatedFromIp = "127.0.0.1" }
            );

            context.ResumeAchievements.AddRange(
                new ResumeAchievement { ResumeId = primaryResume.Id, Title = "Global AI Hackathon 1st Place Winner", Description = "Built an automated job application matcher using open-source models.", OrderIndex = 1, CreatedBy = "system_seeder", CreatedFromIp = "127.0.0.1" }
            );

            // 4. Seed Portfolio
            var portfolio = new Portfolio
            {
                UserId = demoUser.Id,
                CustomSlug = "pratyush-sharma",
                ProfileHeadline = "Staff Full Stack & AI Software Engineer",
                ProfileSummary = "I design and build intuitive AI-first SaaS products.",
                ContactEmail = "pratyush@ascend.ai",
                ContactPhone = "+1 (555) 019-2834",
                Address = "San Francisco, CA",
                ProfilePictureUrl = "https://images.unsplash.com/photo-1534528741775-53994a69daeb?w=400",
                BannerPictureUrl = "https://images.unsplash.com/photo-1618005182384-a83a8bd57fbe?w=1200",
                ThemeName = "modern-dark",
                SEOTitle = "Pratyush Sharma | AI Engineer & Architect",
                SEODescription = "Portfolio of Pratyush Sharma - Staff AI Engineer specializing in .NET and React.",
                IsPublic = true,
                CreatedBy = "system_seeder",
                CreatedFromIp = "127.0.0.1"
            };

            context.Portfolios.Add(portfolio);
            await context.SaveChangesAsync();

            context.PortfolioSocialLinks.AddRange(
                new PortfolioSocialLink { PortfolioId = portfolio.Id, Platform = "GitHub", Url = "https://github.com/Pratyush12-SFA", OrderIndex = 1, CreatedBy = "system_seeder", CreatedFromIp = "127.0.0.1" },
                new PortfolioSocialLink { PortfolioId = portfolio.Id, Platform = "LinkedIn", Url = "https://linkedin.com/in/pratyush-sharma", OrderIndex = 2, CreatedBy = "system_seeder", CreatedFromIp = "127.0.0.1" },
                new PortfolioSocialLink { PortfolioId = portfolio.Id, Platform = "X (Twitter)", Url = "https://x.com/pratyush_dev", OrderIndex = 3, CreatedBy = "system_seeder", CreatedFromIp = "127.0.0.1" }
            );

            context.RecruiterMessages.Add(
                new RecruiterMessage
                {
                    PortfolioId = portfolio.Id,
                    SenderName = "Sarah Jenkins",
                    SenderEmail = "sarah.jenkins@techrecruiter.com",
                    SenderCompany = "Stripe",
                    Message = "Hi Pratyush, loved your AI Career OS project! We have an opening for a Staff Software Architect.",
                    IsRead = false,
                    CreatedBy = "system_seeder",
                    CreatedFromIp = "127.0.0.1"
                }
            );

            // 5. Seed AI Chat Session & Sample Message
            var aiSession = new AIChatSession
            {
                UserId = demoUser.Id,
                Title = "Resume Review & ATS Optimization",
                Context = "User seeking feedback on Senior AI Software Architect resume bullets.",
                CreatedBy = "system_seeder",
                CreatedFromIp = "127.0.0.1"
            };

            context.AIChatSessions.Add(aiSession);
            await context.SaveChangesAsync();

            context.AIChatMessages.AddRange(
                new AIChatMessage { SessionId = aiSession.Id, Role = "user", Content = "How can I improve the impact of my experience bullets for AI Architect roles?", InputTokens = 42, OutputTokens = 0, CreatedBy = "system_seeder", CreatedFromIp = "127.0.0.1" },
                new AIChatMessage { SessionId = aiSession.Id, Role = "assistant", Content = "Great work! Focus on quantifiable metrics. For instance, mention latency reduction, cost savings per API call, or user retention improvements.", InputTokens = 0, OutputTokens = 68, CreatedBy = "system_seeder", CreatedFromIp = "127.0.0.1" }
            );

            // 6. Seed Job Tracker Entries
            var jobApp = new JobApplication
            {
                UserId = demoUser.Id,
                JobTitle = "Staff Software Architect - AI Platform",
                CompanyName = "Linear",
                JobUrl = "https://linear.app/careers/staff-architect",
                Location = "San Francisco / Remote",
                JobType = "Full-time",
                SalaryRange = "$220,000 - $280,000 USD",
                Status = "Interview",
                AppliedDate = DateTime.UtcNow.AddDays(-10),
                Notes = "Passed initial recruiter screen. System design interview scheduled next Tuesday.",
                ATSScore = 94,
                CreatedBy = "system_seeder",
                CreatedFromIp = "127.0.0.1"
            };

            context.JobApplications.Add(jobApp);
            await context.SaveChangesAsync();

            context.InterviewRounds.Add(
                new InterviewRound
                {
                    JobApplicationId = jobApp.Id,
                    RoundNumber = 1,
                    RoundType = "Technical System Design",
                    ScheduledAt = DateTime.UtcNow.AddDays(3),
                    InterviewerName = "Alex Rivera (VP of Eng)",
                    Notes = "Review distributed caching and event-driven architecture patterns.",
                    Outcome = "Pending",
                    CreatedBy = "system_seeder",
                    CreatedFromIp = "127.0.0.1"
                }
            );

            // 7. Seed Notifications
            context.Notifications.AddRange(
                new NotificationEntity
                {
                    UserId = demoUser.Id,
                    Type = "Success",
                    Title = "ATS Score Verified",
                    Message = "Your primary resume achieved an 88% ATS match score for AI Staff Engineer roles.",
                    IsRead = false,
                    ActionUrl = "/dashboard/resume",
                    CreatedBy = "system_seeder",
                    CreatedFromIp = "127.0.0.1"
                },
                new NotificationEntity
                {
                    UserId = demoUser.Id,
                    Type = "Info",
                    Title = "New Recruiter Inquiry",
                    Message = "Sarah Jenkins from Stripe sent a message on your public portfolio.",
                    IsRead = false,
                    ActionUrl = "/dashboard/portfolio",
                    CreatedBy = "system_seeder",
                    CreatedFromIp = "127.0.0.1"
                }
            );

            // 8. Seed Prompt Templates
            context.PromptTemplates.AddRange(
                new PromptTemplate
                {
                    Name = "Resume Improve Template",
                    Feature = "ResumeImprove",
                    SystemPrompt = "You are a professional resume writer. Rewrite the provided resume section to be highly polished, professional, and impact-driven. Improve clarity, structure, and action verbs.",
                    IsActive = true,
                    Version = 1,
                    CreatedBy = "system_seeder",
                    CreatedFromIp = "127.0.0.1",
                    CreatedOn = DateTime.UtcNow
                },
                new PromptTemplate
                {
                    Name = "ATS Optimizer Template",
                    Feature = "ResumeATS",
                    SystemPrompt = "Compare the provided Resume Content and Job Description. Perform a comprehensive ATS matching analysis. Evaluate keywords overlap, missing experience, and write actionable optimization advice. You MUST reply ONLY with a JSON object in this format: { \"score\": 85, \"matchedKeywords\": [\"skill1\", \"skill2\"], \"missingKeywords\": [\"skill3\"], \"recommendations\": [\"rec1\", \"rec2\"] }",
                    IsActive = true,
                    Version = 1,
                    CreatedBy = "system_seeder",
                    CreatedFromIp = "127.0.0.1",
                    CreatedOn = DateTime.UtcNow
                },
                new PromptTemplate
                {
                    Name = "Grammar & Structure Fixer",
                    Feature = "ResumeGrammar",
                    SystemPrompt = "You are a professional editor. Identify and fix any grammatical, spelling, formatting, and structural issues in the provided text. Return ONLY the polished output. Do not add commentaries, notes, or intros.",
                    IsActive = true,
                    Version = 1,
                    CreatedBy = "system_seeder",
                    CreatedFromIp = "127.0.0.1",
                    CreatedOn = DateTime.UtcNow
                },
                new PromptTemplate
                {
                    Name = "STAR Bullet Points Rewriter",
                    Feature = "ResumeBullets",
                    SystemPrompt = "You are an expert resume copywriter. Polish each experience bullet point in the list to be active, using action-verbs and impact metrics. You MUST reply ONLY in a JSON array format of strings, e.g.: [\"rewritten bullet 1\", \"rewritten bullet 2\"]",
                    IsActive = true,
                    Version = 1,
                    CreatedBy = "system_seeder",
                    CreatedFromIp = "127.0.0.1",
                    CreatedOn = DateTime.UtcNow
                },
                new PromptTemplate
                {
                    Name = "Professional Summary Generator",
                    Feature = "ResumeSummary",
                    SystemPrompt = "You are a senior professional resume consultant. Write a high-impact, professional summary (3-4 sentences) for a candidate based on the provided background, technical skills, and experience details. Make it compelling and modern.",
                    IsActive = true,
                    Version = 1,
                    CreatedBy = "system_seeder",
                    CreatedFromIp = "127.0.0.1",
                    CreatedOn = DateTime.UtcNow
                },
                new PromptTemplate
                {
                    Name = "Missing Skills Recommender",
                    Feature = "ResumeSkills",
                    SystemPrompt = "Analyze the candidate's work details and compare it with the target role or description. Suggest 5 to 10 highly relevant skills or keyword tools they should list on their resume. Respond ONLY as a JSON array of strings, e.g.: [\"skill1\", \"skill2\"]",
                    IsActive = true,
                    Version = 1,
                    CreatedBy = "system_seeder",
                    CreatedFromIp = "127.0.0.1",
                    CreatedOn = DateTime.UtcNow
                },
                new PromptTemplate
                {
                    Name = "Premium Career Coach Prompt",
                    Feature = "CareerCoach",
                    SystemPrompt = "You are Antigravity, a premium AI career coach. You help software developers improve their resume, portfolio, projects, and prep for technical coding interviews.",
                    IsActive = true,
                    Version = 1,
                    CreatedBy = "system_seeder",
                    CreatedFromIp = "127.0.0.1",
                    CreatedOn = DateTime.UtcNow
                }
            );

            await context.SaveChangesAsync();
            logger.LogInformation("Development data seeded successfully!");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the development database.");
            throw;
        }
    }
}
