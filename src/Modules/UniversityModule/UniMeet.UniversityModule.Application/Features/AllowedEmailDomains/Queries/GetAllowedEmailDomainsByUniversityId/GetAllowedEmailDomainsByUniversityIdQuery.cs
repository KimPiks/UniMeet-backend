using MediatR;
using UniMeet.UniversityModule.Application.DTOs;
using System.Collections.Generic;

namespace UniMeet.UniversityModule.Application.Features.AllowedEmailDomains.Queries.GetAllowedEmailDomainsByUniversityId;

public record GetAllowedEmailDomainsByUniversityIdQuery(int UniversityId) : IRequest<IEnumerable<AllowedEmailDomainDto>>;