using UniMeet.Shared.Abstractions;

namespace UniMeet.UniversityModule.Application.AllowedEmailDomains.UpdateAllowedEmailDomain;

public record UpdateAllowedEmailDomainCommand(int UniversityId, int DomainId, string? NewDomain) : ICommand;