namespace AIPortfolio.Application.DTOs.Auth;

public sealed record ChangePasswordRequest(string OldPassword, string NewPassword);