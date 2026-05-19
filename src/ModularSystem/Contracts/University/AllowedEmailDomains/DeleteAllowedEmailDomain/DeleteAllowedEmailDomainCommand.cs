using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.University.AllowedEmailDomains.DeleteAllowedEmailDomain;

public record DeleteAllowedEmailDomainCommand(int DomainId) : ICommand;