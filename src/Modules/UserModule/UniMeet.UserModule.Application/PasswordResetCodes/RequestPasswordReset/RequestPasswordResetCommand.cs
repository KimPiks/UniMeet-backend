using UniMeet.Shared.Abstractions;

namespace UniMeet.UserModule.Application.PasswordResetCodes.RequestPasswordReset;

public record RequestPasswordResetCommand(string Email) : ICommand;