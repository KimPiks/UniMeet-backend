using UniMeet.Shared.Abstractions;

namespace UniMeet.UniversityModule.Application.Features.AllowedEmailDomains.Commands.DeleteAllowedEmailDomain;

public record DeleteAllowedEmailDomainCommand(int UniversityId, int DomainId) : IRequest;