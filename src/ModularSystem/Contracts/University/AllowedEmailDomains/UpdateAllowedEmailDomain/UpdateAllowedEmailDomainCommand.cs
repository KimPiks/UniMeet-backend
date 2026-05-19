using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.University.AllowedEmailDomains.UpdateAllowedEmailDomain;

public record UpdateAllowedEmailDomainCommand(int DomainId, string NewDomain) : ICommand;