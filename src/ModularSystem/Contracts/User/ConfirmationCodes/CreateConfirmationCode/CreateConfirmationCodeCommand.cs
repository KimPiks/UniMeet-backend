using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.User.ConfirmationCodes.CreateConfirmationCode;

public record CreateConfirmationCodeCommand(Guid UserId) : ICommand<Guid>;