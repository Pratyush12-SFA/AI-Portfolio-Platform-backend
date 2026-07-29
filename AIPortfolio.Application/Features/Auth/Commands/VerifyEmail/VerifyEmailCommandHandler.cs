using AIPortfolio.Application.Abstractions;
using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Auth.Commands.VerifyEmail;

internal sealed class VerifyEmailCommandHandler : IRequestHandler<VerifyEmailCommand, Result>
{
    private readonly IUserRepository _userRepository;

    public VerifyEmailCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result> Handle(VerifyEmailCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByVerificationTokenAsync(command.Token);
        if (user is null) return Result.NotFound("Invalid or expired verification token.");

        await _userRepository.VerifyEmailAsync(user.Id);
        return Result.Success();
    }
}