using AIPortfolio.Application.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AIPortfolio.API.Endpoints.AI;

internal static class ResumeEndpoints
{
    public static async Task<IResult> ImproveSection(
        [FromBody] ImproveSectionRequest request,
        IResumeAIService resumeAiService,
        IUserInfoAccessor userInfoAccessor)
    {
        if (!userInfoAccessor.IsAuthenticated) return Results.Unauthorized();
        if (string.IsNullOrWhiteSpace(request.SectionContent))
        {
            return Results.BadRequest(new { message = "Section content is required." });
        }

        var result = await resumeAiService.ImproveSectionAsync(request.SectionContent, request.JobDescription);
        return Results.Ok(new { result });
    }

    public static async Task<IResult> AnalyzeATSScore(
        [FromBody] ATSScoreRequest request,
        IResumeAIService resumeAiService,
        IUserInfoAccessor userInfoAccessor)
    {
        if (!userInfoAccessor.IsAuthenticated) return Results.Unauthorized();
        if (string.IsNullOrWhiteSpace(request.ResumeContent) || string.IsNullOrWhiteSpace(request.JobDescription))
        {
            return Results.BadRequest(new { message = "Both resume content and job description are required." });
        }

        var result = await resumeAiService.AnalyzeATSScoreAsync(request.ResumeContent, request.JobDescription);
        return Results.Ok(result);
    }

    public static async Task<IResult> FixGrammar(
        [FromBody] GrammarFixRequest request,
        IResumeAIService resumeAiService,
        IUserInfoAccessor userInfoAccessor)
    {
        if (!userInfoAccessor.IsAuthenticated) return Results.Unauthorized();
        if (string.IsNullOrWhiteSpace(request.Text))
        {
            return Results.BadRequest(new { message = "Text content is required." });
        }

        var result = await resumeAiService.FixGrammarAsync(request.Text);
        return Results.Ok(new { result });
    }

    public static async Task<IResult> RewriteBullets(
        [FromBody] RewriteBulletsRequest request,
        IResumeAIService resumeAiService,
        IUserInfoAccessor userInfoAccessor)
    {
        if (!userInfoAccessor.IsAuthenticated) return Results.Unauthorized();
        if (request.Bullets == null || request.Bullets.Count == 0)
        {
            return Results.BadRequest(new { message = "A list of bullet points is required." });
        }

        var result = await resumeAiService.RewriteBulletPointsAsync(request.Bullets);
        return Results.Ok(new { result });
    }

    public static async Task<IResult> SuggestSkills(
        [FromBody] SuggestSkillsRequest request,
        IResumeAIService resumeAiService,
        IUserInfoAccessor userInfoAccessor)
    {
        if (!userInfoAccessor.IsAuthenticated) return Results.Unauthorized();
        if (string.IsNullOrWhiteSpace(request.ExperienceText) || string.IsNullOrWhiteSpace(request.TargetRole))
        {
            return Results.BadRequest(new { message = "Experience text and target role are required." });
        }

        var result = await resumeAiService.SuggestMissingSkillsAsync(request.ExperienceText, request.TargetRole);
        return Results.Ok(new { result });
    }

    public static async Task<IResult> GenerateSummary(
        [FromBody] SummaryRequest request,
        IResumeAIService resumeAiService,
        IUserInfoAccessor userInfoAccessor)
    {
        if (!userInfoAccessor.IsAuthenticated) return Results.Unauthorized();
        if (string.IsNullOrWhiteSpace(request.ExperienceAndSkills))
        {
            return Results.BadRequest(new { message = "Experience and skills details are required." });
        }

        var result = await resumeAiService.GenerateProfessionalSummaryAsync(request.ExperienceAndSkills);
        return Results.Ok(new { result });
    }
}

public record ImproveSectionRequest(string SectionContent, string? JobDescription);
public record ATSScoreRequest(string ResumeContent, string JobDescription);
public record GrammarFixRequest(string Text);
public record RewriteBulletsRequest(List<string> Bullets);
public record SuggestSkillsRequest(string ExperienceText, string TargetRole);
public record SummaryRequest(string ExperienceAndSkills);
