using AIPortfolio.Application.Abstractions;
using AIPortfolio.Domain.Entites;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AIPortfolio.API.Endpoints.AI;

internal static class ChatEndpoints
{
    public static async Task<IResult> GetSessions(
        IChatService chatService,
        IUserInfoAccessor userInfoAccessor)
    {
        if (!userInfoAccessor.IsAuthenticated) return Results.Unauthorized();

        var sessions = await chatService.GetUserSessionsAsync(userInfoAccessor.UserId);
        return Results.Ok(sessions);
    }

    public static async Task<IResult> CreateSession(
        [FromBody] CreateSessionRequest request,
        IChatService chatService,
        IUserInfoAccessor userInfoAccessor)
    {
        if (!userInfoAccessor.IsAuthenticated) return Results.Unauthorized();

        var session = await chatService.CreateSessionAsync(userInfoAccessor.UserId, request.Title);
        return Results.Ok(session);
    }

    public static async Task<IResult> GetMessages(
        long sessionId,
        IChatService chatService,
        IUserInfoAccessor userInfoAccessor)
    {
        if (!userInfoAccessor.IsAuthenticated) return Results.Unauthorized();

        var messages = await chatService.GetSessionMessagesAsync(sessionId);
        return Results.Ok(messages);
    }

    public static async Task<IResult> SendMessage(
        long sessionId,
        [FromBody] SendMessageRequest request,
        IChatService chatService,
        IUserInfoAccessor userInfoAccessor)
    {
        if (!userInfoAccessor.IsAuthenticated) return Results.Unauthorized();
        if (string.IsNullOrWhiteSpace(request.Message))
        {
            return Results.BadRequest(new { message = "Message content is required" });
        }

        var assistantMsg = await chatService.SendMessageAsync(sessionId, request.Message);
        return Results.Ok(assistantMsg);
    }
}

public record CreateSessionRequest(string Title);
public record SendMessageRequest(string Message);
