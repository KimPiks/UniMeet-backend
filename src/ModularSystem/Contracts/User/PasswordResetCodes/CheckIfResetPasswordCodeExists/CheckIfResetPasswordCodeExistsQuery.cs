using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.User.PasswordResetCodes.CheckIfResetPasswordCodeExists;

public record CheckIfResetPasswordCodeExistsQuery(Guid Code) : IQuery<bool>;