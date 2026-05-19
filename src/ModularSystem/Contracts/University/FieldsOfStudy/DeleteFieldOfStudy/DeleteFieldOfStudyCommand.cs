using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.University.FieldsOfStudy.DeleteFieldOfStudy;

public record DeleteFieldOfStudyCommand(int FieldOfStudyId) : ICommand;