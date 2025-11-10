using UniMeet.Shared.Abstractions;

namespace UniMeet.UniversityModule.Application.FieldsOfStudy.AddFieldOfStudy;

public record AddFieldOfStudyCommand(int DepartmentId, string FieldOfStudyName) : ICommand;