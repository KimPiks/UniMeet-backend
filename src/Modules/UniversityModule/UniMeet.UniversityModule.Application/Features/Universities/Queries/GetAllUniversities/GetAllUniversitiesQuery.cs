using UniMeet.UniversityModule.Application.DTOs;
using System.Collections.Generic;
using UniMeet.Shared.Abstractions;

namespace UniMeet.UniversityModule.Application.Universities.Queries.GetAllUniversities;
public record GetAllUniversitiesQuery :  IRequest<IEnumerable<UniversityDto>>;