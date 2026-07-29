using AIPortfolio.Application.Abstractions;
using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Auth.Commands.ForgotPassword;

internal sealed class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, Result>
{
    private readonly IUserRepository _userRepository;

    public ForgotPasswordCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result> Handle(ForgotPasswordCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(command.Email);
        if (user is null) return Result.Success();

        var token = Guid.NewGuid().ToString("N");
        var expires = DateTime.UtcNow.AddHours(1);
        await _userRepository.UpdateSecurityTokensAsync(user.Id, token, expires, user.VerificationToken,
            user.VerificationExpiresAt);

        Console.WriteLine($"[EMAIL MOCK] Password Reset link: http://localhost:5173/reset-password?token={token}");

        return Result.Success();
    }
}