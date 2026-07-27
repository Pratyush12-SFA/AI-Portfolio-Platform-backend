using AIPortfolio.Application.Abstractions;
using AIPortfolio.Application.DTOs.AI;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace AIPortfolio.Infrastructure.AI;

public sealed class ResumeAIService : IResumeAIService
{
    private readonly IAIProvider _aiProvider;
    private readonly IPromptRepository _promptRepository;

    public ResumeAIService(
        IAIProvider aiProvider,
        IPromptRepository promptRepository)
    {
        _aiProvider = aiProvider ?? throw new ArgumentNullException(nameof(aiProvider));
        _promptRepository = promptRepository ?? throw new ArgumentNullException(nameof(promptRepository));
    }

    public async Task<string> ImproveSectionAsync(string sectionContent, string? jobDescription)
    {
        string systemPrompt = "You are a professional resume writer. Rewrite the provided resume section to be highly polished, professional, and impact-driven. Improve clarity, structure, and action verbs.";
        var promptTemplate = await _promptRepository.GetActiveTemplateByFeatureAsync("ResumeImprove");
        if (promptTemplate is not null)
        {
            systemPrompt = promptTemplate.SystemPrompt;
        }

        string userPrompt = $"Resume Section Content:\n{sectionContent}";
        if (!string.IsNullOrWhiteSpace(jobDescription))
        {
            userPrompt += $"\n\nOptimize specifically for this Target Job Description:\n{jobDescription}";
        }

        return await _aiProvider.GenerateAsync(
            systemPrompt,
            userPrompt,
            "gemini-2.5-pro");
    }

    public async Task<ATSResultDto> AnalyzeATSScoreAsync(string resumeContent, string jobDescription)
    {
        string systemPrompt = "Compare the provided Resume Content and Job Description. Perform a comprehensive ATS matching analysis. Evaluate keywords overlap, missing experience, and write actionable optimization advice. You MUST reply ONLY with a JSON object in this format: { \"score\": 85, \"matchedKeywords\": [\"skill1\", \"skill2\"], \"missingKeywords\": [\"skill3\"], \"recommendations\": [\"rec1\", \"rec2\"] }";
        var promptTemplate = await _promptRepository.GetActiveTemplateByFeatureAsync("ResumeATS");
        if (promptTemplate is not null)
        {
            systemPrompt = promptTemplate.SystemPrompt;
        }

        string userPrompt = $"Resume Content:\n{resumeContent}\n\nTarget Job Description:\n{jobDescription}";
        string responseJson = await _aiProvider.GenerateAsync(
            systemPrompt,
            userPrompt,
            "gemini-2.5-pro",
            requestJson: true);

        try
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var result = JsonSerializer.Deserialize<ATSResultDto>(responseJson, options);
            return result ?? new ATSResultDto { Score = 0, Recommendations = new[] { "Failed to parse ATS response" } };
        }
        catch
        {
            return new ATSResultDto
            {
                Score = 0,
                Recommendations = new[] { "Error decoding structured ATS response from Gemini API.", "Ensure your resume content is fully readable." }
            };
        }
    }

    public async Task<string> FixGrammarAsync(string text)
    {
        string systemPrompt = "You are a professional editor. Identify and fix any grammatical, spelling, formatting, and structural issues in the provided text. Return ONLY the polished output. Do not add commentaries, notes, or intros.";
        var promptTemplate = await _promptRepository.GetActiveTemplateByFeatureAsync("ResumeGrammar");
        if (promptTemplate is not null)
        {
            systemPrompt = promptTemplate.SystemPrompt;
        }

        return await _aiProvider.GenerateAsync(
            systemPrompt,
            text,
            "gemini-2.5-flash");
    }

    public async Task<IEnumerable<string>> RewriteBulletPointsAsync(IEnumerable<string> bullets)
    {
        string systemPrompt = "You are an expert resume copywriter. Polish each experience bullet point in the list to be active, using action-verbs and impact metrics. You MUST reply ONLY in a JSON array format of strings, e.g.: [\"rewritten bullet 1\", \"rewritten bullet 2\"]";
        var promptTemplate = await _promptRepository.GetActiveTemplateByFeatureAsync("ResumeBullets");
        if (promptTemplate is not null)
        {
            systemPrompt = promptTemplate.SystemPrompt;
        }

        string userPrompt = $"Bullet Points List:\n" + string.Join("\n- ", bullets);
        string responseJson = await _aiProvider.GenerateAsync(
            systemPrompt,
            userPrompt,
            "gemini-2.5-flash",
            requestJson: true);

        try
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<List<string>>(responseJson, options) ?? new List<string>(bullets);
        }
        catch
        {
            return bullets;
        }
    }

    public async Task<string> GenerateProfessionalSummaryAsync(string experienceAndSkills)
    {
        string systemPrompt = "You are a senior professional resume consultant. Write a high-impact, professional summary (3-4 sentences) for a candidate based on the provided background, technical skills, and experience details. Make it compelling and modern.";
        var promptTemplate = await _promptRepository.GetActiveTemplateByFeatureAsync("ResumeSummary");
        if (promptTemplate is not null)
        {
            systemPrompt = promptTemplate.SystemPrompt;
        }

        return await _aiProvider.GenerateAsync(
            systemPrompt,
            $"Candidate Background:\n{experienceAndSkills}",
            "gemini-2.5-flash");
    }

    public async Task<IEnumerable<string>> SuggestMissingSkillsAsync(string experienceText, string targetRole)
    {
        string systemPrompt = "Analyze the candidate's work details and compare it with the target role or description. Suggest 5 to 10 highly relevant skills or keyword tools they should list on their resume. Respond ONLY as a JSON array of strings, e.g.: [\"skill1\", \"skill2\"]";
        var promptTemplate = await _promptRepository.GetActiveTemplateByFeatureAsync("ResumeSkills");
        if (promptTemplate is not null)
        {
            systemPrompt = promptTemplate.SystemPrompt;
        }

        string userPrompt = $"Candidate Experience details:\n{experienceText}\n\nTarget Role/Keywords:\n{targetRole}";
        string responseJson = await _aiProvider.GenerateAsync(
            systemPrompt,
            userPrompt,
            "gemini-2.5-flash",
            requestJson: true);

        try
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<List<string>>(responseJson, options) ?? new List<string>();
        }
        catch
        {
            return new List<string>();
        }
    }
}
