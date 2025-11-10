using UniMeet.Shared.Abstractions;

namespace UniMeet.UniversityModule.Application.Features.FieldsOfStudy.Commands.UpdateFieldOfStudy;

public record UpdateFieldOfStudyCommand(int UniversityId, int DepartmentId, int FieldOfStudyId, string? NewFieldOfStudyName) : IRequest;