using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.University.FieldsOfStudy.AddFieldOfStudy;

public record AddFieldOfStudyCommand(int DepartmentId, string FieldOfStudyName) : ICommand;