using UniMeet.Shared.Abstractions;

namespace UniMeet.UniversityModule.Application.Universities.GetAllUniversities;

public record GetAllUniversitiesQuery(int Offset, int Limit) :  IQuery<IEnumerable<UniversityDto>>;