using UniMeet.Shared.Abstractions;

namespace UniMeet.UniversityModule.Application.AllowedEmailDomains.GetAllowedEmailDomainById;

public record GetAllowedEmailDomainByIdQuery(int DomainId) : IQuery<AllowedEmailDomainDto?>;