using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.University.FieldsOfStudy.GetFieldsOfStudyByDepartmentId;

public record GetFieldsOfStudyByDepartmentIdQuery(int DepartmentId, int Offset, int Limit) : IQuery<IEnumerable<FieldOfStudyDto>>;