using MediatR;
using UniMeet.UniversityModule.Application.DTOs;
using System.Collections.Generic;

namespace UniMeet.UniversityModule.Application.Universities.Queries.GetAllUniversities;
public record GetAllUniversitiesQuery :  IRequest<IEnumerable<UniversityDto>>;