using UniMeet.Shared.Abstractions;

namespace UniMeet.UniversityModule.Application.Features.AllowedEmailDomains.Commands.UpdateAllowedEmailDomain;

public record UpdateAllowedEmailDomainCommand(int UniversityId, int DomainId, string? NewDomain) : IRequest;