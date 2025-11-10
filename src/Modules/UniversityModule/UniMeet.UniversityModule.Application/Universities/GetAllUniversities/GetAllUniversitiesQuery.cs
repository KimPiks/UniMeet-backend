using UniMeet.Shared.Abstractions;

namespace UniMeet.UniversityModule.Application.Universities.GetAllUniversities;

public record GetAllUniversitiesQuery :  IQuery<IEnumerable<UniversityDto>>;