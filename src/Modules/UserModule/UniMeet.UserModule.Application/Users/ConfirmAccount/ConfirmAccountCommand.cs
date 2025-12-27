using UniMeet.Shared.Abstractions;

namespace UniMeet.UserModule.Application.Users.ConfirmAccount;

public record ConfirmAccountCommand(Guid ConfirmationCode) : ICommand;