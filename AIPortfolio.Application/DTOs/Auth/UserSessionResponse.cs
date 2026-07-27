namespace AIPortfolio.Application.DTOs.Auth;

public sealed class UserSessionResponse
{
    public long Id { get; init; }

    public string? DeviceName { get; init; } 
 
    public string? CreatedFromIp { get; init; } 
    public DateTime LoginAt { get; init; }
    
    public bool IsCurrentSession { get; init; }
}
