using UniMeet.Shared.Abstractions;

namespace UniMeet.UniversityModule.Application.FieldsOfStudy.UpdateFieldOfStudy;

public record UpdateFieldOfStudyCommand(int FieldOfStudyId, string NewFieldOfStudyName) : ICommand;