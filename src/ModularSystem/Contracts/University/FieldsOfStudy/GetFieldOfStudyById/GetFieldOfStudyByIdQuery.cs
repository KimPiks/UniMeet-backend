using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.University.FieldsOfStudy.GetFieldOfStudyById;

public record GetFieldOfStudyByIdQuery(int FieldOfStudyId) : IQuery<FieldOfStudyDto?>;