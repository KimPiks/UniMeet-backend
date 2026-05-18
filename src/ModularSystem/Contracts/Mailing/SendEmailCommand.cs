using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.Mailing;

public record SendEmailCommand(
    string To,
    EmailType EmailType,
    List<EmailParameter> Parameters)
    : ICommand;
