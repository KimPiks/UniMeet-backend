using UniMeet.Shared.Abstractions;

namespace UniMeet.UniversityModule.Application.FieldsOfStudy.GetFieldsOfStudyByDepartmentId;

public record GetFieldsOfStudyByDepartmentIdQuery(int UniversityId, int DepartmentId) : IRequest<IEnumerable<FieldOfStudyDto>>;