using UniMeet.Shared.Abstractions;
using UniMeet.UniversityModule.Application.DTOs;

namespace UniMeet.UniversityModule.Application.Universities.GetAllUniversities;

public record GetAllUniversitiesQuery :  IRequest<IEnumerable<UniversityDto>>;