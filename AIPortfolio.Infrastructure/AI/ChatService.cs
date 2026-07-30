using System.Text;
using AIPortfolio.Application.Abstractions;
using AIPortfolio.Domain.Entites;

namespace AIPortfolio.Infrastructure.AI;

public sealed class ChatService : IChatService
{
    private readonly IAIProvider _aiProvider;
    private readonly IChatRepository _chatRepository;
    private readonly IPromptRepository _promptRepository;
    private readonly IUserRepository _userRepository;
    private readonly IPortfolioRepository _portfolioRepository;

    public ChatService(
        IChatRepository chatRepository,
        IPromptRepository promptRepository,
        IAIProvider aiProvider,
        IUserRepository userRepository,
        IPortfolioRepository portfolioRepository)
    {
        _chatRepository = chatRepository ?? throw new ArgumentNullException(nameof(chatRepository));
        _promptRepository = promptRepository ?? throw new ArgumentNullException(nameof(promptRepository));
        _aiProvider = aiProvider ?? throw new ArgumentNullException(nameof(aiProvider));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _portfolioRepository = portfolioRepository ?? throw new ArgumentNullException(nameof(portfolioRepository));
    }

    public async Task<AIChatSession> CreateSessionAsync(long userId, string title)
    {
        var session = new AIChatSession
        {
            UserId = userId,
            Title = string.IsNullOrWhiteSpace(title) ? "New Coaching Chat" : title,
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow
        };

        var id = await _chatRepository.CreateSessionAsync(session);
        session.Id = id;
        return session;
    }

    public async Task<IEnumerable<AIChatSession>> GetUserSessionsAsync(long userId)
    {
        return await _chatRepository.ListSessionsAsync(userId);
    }

    public async Task<IEnumerable<AIChatMessage>> GetSessionMessagesAsync(long sessionId)
    {
        return await _chatRepository.ListMessagesAsync(sessionId);
    }

    public async Task<AIChatMessage> SendMessageAsync(long sessionId, string userContent)
    {
        var session = await _chatRepository.GetSessionByIdAsync(sessionId);
        if (session is null) throw new KeyNotFoundException($"Coaching session ID {sessionId} not found.");

        // 1. Log the user's message
        var userMessage = new AIChatMessage
        {
            SessionId = sessionId,
            Role = "User",
            Content = userContent,
            InputTokens = TokenCounter.EstimateTokens(userContent),
            OutputTokens = 0,
            CreatedOn = DateTime.UtcNow
        };
        await _chatRepository.CreateMessageAsync(userMessage);

        // 2. Fetch full conversation history for context
        var history = await _chatRepository.ListMessagesAsync(sessionId);

        // 3. Load dynamic system prompt or fall back to standard premium prompt
        var systemPrompt =
            "You are Antigravity, a premium AI career coach. You help software developers improve their resume, portfolio, projects, and prep for technical coding interviews.";
        var promptTemplate = await _promptRepository.GetActiveTemplateByFeatureAsync("CareerCoach");
        if (promptTemplate is not null) systemPrompt = promptTemplate.SystemPrompt;

        // 4. Inject user resume data into the system prompt for personalized coaching
        var user = await _userRepository.GetByIdAsync(session.UserId);
        var profile = await _portfolioRepository.GetProfileByUserIdAsync(session.UserId);
        var experiences = await _portfolioRepository.GetExperiencesByUserIdAsync(session.UserId);
        var educations = await _portfolioRepository.GetEducationsByUserIdAsync(session.UserId);
        var skills = await _portfolioRepository.GetSkillsByUserIdAsync(session.UserId);
        var projects = await _portfolioRepository.GetProjectsByUserIdAsync(session.UserId);
        var certifications = await _portfolioRepository.GetCertificationsByUserIdAsync(session.UserId);

        var userContext = new StringBuilder();
        userContext.AppendLine("## User Profile Context");
        userContext.AppendLine($"Name: {profile?.FullName ?? user?.Email ?? "Unknown"}");
        if (!string.IsNullOrWhiteSpace(profile?.Headline))
            userContext.AppendLine($"Headline: {profile.Headline}");
        if (!string.IsNullOrWhiteSpace(profile?.Summary))
            userContext.AppendLine($"Summary: {profile.Summary}");
        if (experiences.Any())
        {
            userContext.AppendLine("\n### Work Experience");
            foreach (var exp in experiences)
                userContext.AppendLine($"- {exp.JobTitle} at {exp.CompanyName} ({exp.StartDate:yyyy-MM} - {(exp.IsCurrent ? "Present" : exp.EndDate?.ToString("yyyy-MM"))})");
        }
        if (educations.Any())
        {
            userContext.AppendLine("\n### Education");
            foreach (var edu in educations)
                userContext.AppendLine($"- {edu.Degree} in {edu.FieldOfStudy ?? ""} at {edu.Institution}");
        }
        if (skills.Any())
        {
            userContext.AppendLine("\n### Skills");
            userContext.AppendLine(string.Join(", ", skills.Select(s => s.Name)));
        }
        if (projects.Any())
        {
            userContext.AppendLine("\n### Projects");
            foreach (var proj in projects)
                userContext.AppendLine($"- {proj.Title}: {proj.Description}");
        }
        if (certifications.Any())
        {
            userContext.AppendLine("\n### Certifications");
            foreach (var cert in certifications)
                userContext.AppendLine($"- {cert.Name} ({cert.IssueDate?.ToString("yyyy-MM")})");
        }
        systemPrompt = $"{systemPrompt}\n\n{userContext}";

        // 5. Construct user/history chat stream prompt
        var conversationContext = new StringBuilder();
        foreach (var msg in
                 history.Take(history.Count() - 1)) // skip the user message we just added to send as current userPrompt
            conversationContext.AppendLine($"{msg.Role}: {msg.Content}");
        conversationContext.AppendLine($"User: {userContent}");

        // 6. Query Gemini Provider using gemini-2.5-flash for coaching chat
        var modelName = "gemini-2.5-flash";
        var responseText = await _aiProvider.GenerateAsync(
            systemPrompt,
            conversationContext.ToString(),
            modelName);

        // 6. Log and save assistant response message
        var assistantMessage = new AIChatMessage
        {
            SessionId = sessionId,
            Role = "Assistant",
            Content = responseText,
            InputTokens = TokenCounter.EstimateTokens(conversationContext.ToString()),
            OutputTokens = TokenCounter.EstimateTokens(responseText),
            CreatedOn = DateTime.UtcNow
        };
        await _chatRepository.CreateMessageAsync(assistantMessage);

        return assistantMessage;
    }
}