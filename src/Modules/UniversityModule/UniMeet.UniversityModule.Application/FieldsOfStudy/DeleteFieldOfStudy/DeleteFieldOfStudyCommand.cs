using UniMeet.Shared.Abstractions;

namespace UniMeet.UniversityModule.Application.FieldsOfStudy.DeleteFieldOfStudy;

public record DeleteFieldOfStudyCommand(int UniversityId, int DepartmentId, int FieldOfStudyId) : ICommand;