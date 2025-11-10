using UniMeet.Shared.Abstractions;

namespace UniMeet.UniversityModule.Application.Universities.GetAllUniversities;

public record GetAllUniversitiesQuery :  IRequest<IEnumerable<UniversityDto>>;