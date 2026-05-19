using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.University.Universities.GetUniversityById;

public record GetUniversityByIdQuery(int UniversityId) : IQuery<UniversityDto?>;