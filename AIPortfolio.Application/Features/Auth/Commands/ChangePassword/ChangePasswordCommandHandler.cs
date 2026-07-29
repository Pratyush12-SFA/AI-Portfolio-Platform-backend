using AIPortfolio.Application.Abstractions;
using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Auth.Commands.ChangePassword;

internal sealed class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Result>
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserRepository _userRepository;

    public ChangePasswordCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result> Handle(ChangePasswordCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(command.UserId);
        if (user is null || string.IsNullOrEmpty(user.PasswordHash)) return Result.NotFound("User not found.");

        if (!_passwordHasher.Verify(command.OldPassword, user.PasswordHash))
            return Result.Error("Incorrect old password.");

        var hashed = _passwordHasher.Hash(command.NewPassword);
        await _userRepository.UpdatePasswordAsync(user.Id, hashed);

        return Result.Success();
    }
}