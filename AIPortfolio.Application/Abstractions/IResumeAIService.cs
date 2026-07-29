using AIPortfolio.Application.DTOs.AI;

namespace AIPortfolio.Application.Abstractions;

public interface IResumeAIService
{
    Task<string> ImproveSectionAsync(string sectionContent, string? jobDescription);
    Task<ATSResultDto> AnalyzeATSScoreAsync(string resumeContent, string jobDescription);
    Task<string> FixGrammarAsync(string text);
    Task<IEnumerable<string>> RewriteBulletPointsAsync(IEnumerable<string> bullets);
    Task<string> GenerateProfessionalSummaryAsync(string experienceAndSkills);
    Task<IEnumerable<string>> SuggestMissingSkillsAsync(string experienceText, string targetRole);
}