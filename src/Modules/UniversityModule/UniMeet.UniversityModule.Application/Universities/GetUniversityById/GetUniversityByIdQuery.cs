using UniMeet.Shared.Abstractions;

namespace UniMeet.UniversityModule.Application.Universities.GetUniversityById;

public record GetUniversityByIdQuery(int UniversityId) : IQuery<UniversityDto?>;