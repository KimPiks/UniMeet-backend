using UniMeet.Shared.Abstractions;

namespace UniMeet.UniversityModule.Application.AllowedEmailDomains.DeleteAllowedEmailDomain;

public record DeleteAllowedEmailDomainCommand(int UniversityId, int DomainId) : IRequest;