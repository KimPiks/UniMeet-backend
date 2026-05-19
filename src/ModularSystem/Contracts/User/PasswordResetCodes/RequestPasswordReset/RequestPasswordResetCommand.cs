using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.User.PasswordResetCodes.RequestPasswordReset;

public record RequestPasswordResetCommand(string Email) : ICommand;