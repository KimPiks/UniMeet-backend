using UniMeet.Shared.Abstractions;

namespace UniMeet.UniversityModule.Application.AllowedEmailDomains.GetAllowedEmailDomainById;

public record GetAllowedEmailDomainByIdQuery(int UniversityId, int DomainId) : IQuery<AllowedEmailDomainDto?>;