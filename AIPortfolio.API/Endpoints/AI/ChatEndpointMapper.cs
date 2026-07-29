using AIPortfolio.API.Common;

namespace AIPortfolio.API.Endpoints.AI;

internal sealed class ChatEndpointMapper : IEndpointMapper
{
    public void Map(IEndpointRouteBuilder endpointRouteBuilder)
    {
        ArgumentNullException.ThrowIfNull(endpointRouteBuilder);

        var chatGroup = endpointRouteBuilder.MapGroup("/api/ai/chat")
            .WithTags("AI Career Coach")
            .RequireAuthorization();

        chatGroup.MapGet("sessions", ChatEndpoints.GetSessions)
            .WithName("GetChatSessions")
            .WithDescription("Get all AI coaching sessions for the current user");

        chatGroup.MapPost("sessions", ChatEndpoints.CreateSession)
            .WithName("CreateChatSession")
            .WithDescription("Create a new AI coaching session");

        chatGroup.MapGet("sessions/{sessionId:long}/messages", ChatEndpoints.GetMessages)
            .WithName("GetChatMessages")
            .WithDescription("Get all messages within an AI coaching session");

        chatGroup.MapPost("sessions/{sessionId:long}/messages", ChatEndpoints.SendMessage)
            .WithName("SendChatMessage")
            .WithDescription("Send a message and get a response from the AI career coach");
    }
}