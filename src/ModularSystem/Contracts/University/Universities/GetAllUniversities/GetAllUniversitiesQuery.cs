using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.University.Universities.GetAllUniversities;

public record GetAllUniversitiesQuery(int Offset, int Limit) :  IQuery<IEnumerable<UniversityDto>>;