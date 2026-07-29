namespace AIPortfolio.Application.DTOs.Auth;

public sealed record ResetPasswordRequest(string Token, string NewPassword);