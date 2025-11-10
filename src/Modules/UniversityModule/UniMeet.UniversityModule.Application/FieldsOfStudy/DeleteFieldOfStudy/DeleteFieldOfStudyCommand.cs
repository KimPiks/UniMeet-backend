using UniMeet.Shared.Abstractions;

namespace UniMeet.UniversityModule.Application.FieldsOfStudy.DeleteFieldOfStudy;

public record DeleteFieldOfStudyCommand(int FieldOfStudyId) : ICommand;