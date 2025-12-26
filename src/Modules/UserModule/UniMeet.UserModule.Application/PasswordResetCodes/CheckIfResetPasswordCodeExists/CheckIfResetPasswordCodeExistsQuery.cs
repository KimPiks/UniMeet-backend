using UniMeet.Shared.Abstractions;

namespace UniMeet.UserModule.Application.PasswordResetCodes.CheckIfResetPasswordCodeExists;

public record CheckIfResetPasswordCodeExistsQuery(Guid Code) : IQuery<bool>;