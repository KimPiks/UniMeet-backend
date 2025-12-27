using UniMeet.Shared.Abstractions;

namespace UniMeet.UserModule.Application.ConfirmationCodes.CreateConfirmationCode;

public record CreateConfirmationCodeCommand(Guid UserId) : ICommand<Guid>;