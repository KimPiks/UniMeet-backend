using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.User.Users.ConfirmAccount;

public record ConfirmAccountCommand(Guid ConfirmationCode) : ICommand;