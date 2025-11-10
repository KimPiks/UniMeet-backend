using UniMeet.Shared.Abstractions;

namespace UniMeet.UniversityModule.Application.Features.AllowedEmailDomains.Commands.AddAllowedEmailDomain;

public record AddAllowedEmailDomainCommand(int UniversityId, string Domain) : IRequest;