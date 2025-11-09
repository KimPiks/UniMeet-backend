using MediatR;
using UniMeet.UniversityModule.Application.DTOs;

namespace UniMeet.UniversityModule.Application.Features.AllowedEmailDomains.Queries.GetAllowedEmailDomainById;

public record GetAllowedEmailDomainByIdQuery(int UniversityId, int DomainId) : IRequest<AllowedEmailDomainDto?>;