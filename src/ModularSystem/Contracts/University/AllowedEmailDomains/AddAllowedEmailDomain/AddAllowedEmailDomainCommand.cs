using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.University.AllowedEmailDomains.AddAllowedEmailDomain;

public record AddAllowedEmailDomainCommand(int UniversityId, string Domain) : ICommand;