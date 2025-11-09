using MediatR;
using UniMeet.UniversityModule.Application.DTOs;
using System.Collections.Generic;

namespace UniMeet.UniversityModule.Application.Universities.Queries.GetUniversityById;
public record GetUniversityByIdQuery(int UniversityId) : IRequest<UniversityDto?>;