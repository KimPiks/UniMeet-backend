using UniMeet.Shared.Abstractions;
using UniMeet.UniversityModule.Application.DTOs;

namespace UniMeet.UniversityModule.Application.Universities.GetUniversityById;

public record GetUniversityByIdQuery(int UniversityId) : IRequest<UniversityDto?>;