using UniMeet.Shared.Abstractions;

namespace UniMeet.UniversityModule.Application.Features.FieldsOfStudy.Commands.DeleteFieldOfStudy;

public record DeleteFieldOfStudyCommand(int UniversityId, int DepartmentId, int FieldOfStudyId) : IRequest;