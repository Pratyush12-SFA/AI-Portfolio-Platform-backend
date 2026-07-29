using AIPortfolio.Application.Abstractions;

namespace AIPortfolio.Infrastructure.AI;

public sealed class DevelopmentAIProvider : IAIProvider
{
    public Task<string> GenerateAsync(
        string systemPrompt,
        string userPrompt,
        string modelName,
        bool requestJson = false)
    {
        var promptLower = systemPrompt.ToLowerInvariant() + " " + userPrompt.ToLowerInvariant();

        if (requestJson || promptLower.Contains("json"))
        {
            if (promptLower.Contains("ats") || promptLower.Contains("score"))
                return Task.FromResult(@"{
                    ""score"": 88,
                    ""matchedKeywords"": ["".NET 10"", ""C#"", ""React 19"", ""Entity Framework Core"", ""SQL Server"", ""Minimal APIs""],
                    ""missingKeywords"": [""Redis"", ""Docker"", ""AWS"", ""CI/CD""],
                    ""recommendations"": [
                        ""Incorporate cloud hosting details specifically mentioning AWS or Azure."",
                        ""Add containerization experience with Docker under the core projects."",
                        ""List CI/CD automation experience with GitHub Actions.""
                    ]
                }");

            if (promptLower.Contains("bullet"))
                return Task.FromResult(@"[
                    ""Spearheaded development of high-performance ASP.NET Core microservices, reducing query latency by 45%."",
                    ""Designed and implemented responsive frontend interfaces using React 19, TypeScript, and state management, increasing user engagement by 20%."",
                    ""Engineered database schemas and optimized EF Core query patterns, reducing database CPU load by 35%.""
                ]");

            if (promptLower.Contains("skill"))
                return Task.FromResult(@"[
                    ""React 19"",
                    ""ASP.NET Core"",
                    ""Entity Framework Core"",
                    ""Microsoft SQL Server"",
                    ""TypeScript"",
                    ""Docker"",
                    ""AWS"",
                    ""Redis"",
                    ""Git"",
                    ""CI/CD""
                ]");
        }

        if (promptLower.Contains("summary") || promptLower.Contains("resumesummary"))
            return Task.FromResult(
                "Highly skilled and results-oriented Full Stack .NET Developer with 5+ years of experience designing, building, and deploying scalable web applications. Proficient in ASP.NET Core, React 19, and cloud technologies. Proven track record of optimizing database performance and implementing robust, clean code architectures.");

        if (promptLower.Contains("grammar") || promptLower.Contains("resumegrammar"))
            return Task.FromResult(
                "Polished: Worked on developing the core platform architecture and fixed several critical bugs to improve overall application performance.");

        if (promptLower.Contains("improve") || promptLower.Contains("resumeimprove"))
            return Task.FromResult(
                "Polished Section: Experienced Software Engineer specializing in designing and implementing scalable backend services using .NET 10 and modern React 19 frontends. Collaborated with cross-functional teams to deliver secure, high-traffic APIs.");

        if (promptLower.Contains("coach") || promptLower.Contains("careercoach"))
            return Task.FromResult(
                "Hi Pratyush! As your career coach, I reviewed your profile. Your experience with .NET Core and React is impressive. To stand out for senior roles, I suggest emphasizing your impact using metrics (e.g., 'reduced latency by 30%') and adding containerization experience with Docker. Let's discuss your next steps!");

        return Task.FromResult(
            "I am Antigravity, your development AI assistant. Let me know how I can help you with your resume, portfolio, or coding interviews!");
    }

    public async IAsyncEnumerable<string> StreamAsync(
        string systemPrompt,
        string userPrompt,
        string modelName)
    {
        var response = await GenerateAsync(systemPrompt, userPrompt, modelName);
        var words = response.Split(' ');
        foreach (var word in words)
        {
            yield return word + " ";
            await Task.Delay(30);
        }
    }
}