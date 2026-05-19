using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.User.PasswordResetCodes.ResetPassword;

public record ResetPasswordCommand(Guid Code, string NewPassword) : ICommand;