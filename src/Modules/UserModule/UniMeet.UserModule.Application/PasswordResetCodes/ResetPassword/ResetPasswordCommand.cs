using UniMeet.Shared.Abstractions;

namespace UniMeet.UserModule.Application.PasswordResetCodes.ResetPassword;

public record ResetPasswordCommand(Guid Code, string NewPassword) : ICommand;