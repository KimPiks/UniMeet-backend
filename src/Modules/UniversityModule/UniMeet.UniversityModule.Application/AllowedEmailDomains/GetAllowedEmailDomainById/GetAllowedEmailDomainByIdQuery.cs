using UniMeet.Shared.Abstractions;
using UniMeet.UniversityModule.Application.DTOs;

namespace UniMeet.UniversityModule.Application.AllowedEmailDomains.GetAllowedEmailDomainById;

public record GetAllowedEmailDomainByIdQuery(int UniversityId, int DomainId) : IRequest<AllowedEmailDomainDto?>;