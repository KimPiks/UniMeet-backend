using UniMeet.Shared.Abstractions;

namespace UniMeet.UniversityModule.Application.FieldsOfStudy.GetFieldsOfStudyByDepartmentId;

public record GetFieldsOfStudyByDepartmentIdQuery(int DepartmentId, int Offset, int Limit) : IQuery<IEnumerable<FieldOfStudyDto>>;