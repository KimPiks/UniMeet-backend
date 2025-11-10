using UniMeet.Shared.Abstractions;

namespace UniMeet.UniversityModule.Application.AllowedEmailDomains.AddAllowedEmailDomain;

public record AddAllowedEmailDomainCommand(int UniversityId, string Domain) : ICommand;