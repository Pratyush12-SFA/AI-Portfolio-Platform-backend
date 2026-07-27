using AIPortfolio.API.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AIPortfolio.API.Endpoints.AI;

internal sealed class ResumeEndpointMapper : IEndpointMapper
{
    public void Map(IEndpointRouteBuilder endpointRouteBuilder)
    {
        ArgumentNullException.ThrowIfNull(endpointRouteBuilder);

        var resumeGroup = endpointRouteBuilder.MapGroup("/api/ai/resume")
            .WithTags("AI Resume Assistant")
            .RequireAuthorization();

        resumeGroup.MapPost("improve-section", ResumeEndpoints.ImproveSection)
            .WithName("ImproveResumeSection")
            .WithDescription("Polish a resume section based on job description context");

        resumeGroup.MapPost("ats-score", ResumeEndpoints.AnalyzeATSScore)
            .WithName("AnalyzeATSScore")
            .WithDescription("Compare resume content against a job description for ATS matching percentage");

        resumeGroup.MapPost("grammar-fix", ResumeEndpoints.FixGrammar)
            .WithName("FixResumeGrammar")
            .WithDescription("Correct grammatical and spelling errors in a segment");

        resumeGroup.MapPost("rewrite-bullets", ResumeEndpoints.RewriteBullets)
            .WithName("RewriteResumeBullets")
            .WithDescription("Optimize experience bullet points with impact-driven action formatting");

        resumeGroup.MapPost("suggest-skills", ResumeEndpoints.SuggestSkills)
            .WithName("SuggestMissingSkills")
            .WithDescription("Analyze experience text and suggest relevant skills for a target role");

        resumeGroup.MapPost("summary", ResumeEndpoints.GenerateSummary)
            .WithName("GenerateResumeSummary")
            .WithDescription("Generate a high-impact professional summary header");
    }
}
