using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.University.FieldsOfStudy.UpdateFieldOfStudy;

public record UpdateFieldOfStudyCommand(int FieldOfStudyId, string NewFieldOfStudyName) : ICommand;