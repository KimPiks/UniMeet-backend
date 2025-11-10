using UniMeet.Shared.Abstractions;
using UniMeet.UniversityModule.Application.DTOs;

namespace UniMeet.UniversityModule.Application.FieldsOfStudy.GetFieldsOfStudyByDepartmentId;

public record GetFieldsOfStudyByDepartmentIdQuery(int UniversityId, int DepartmentId) : IRequest<IEnumerable<FieldOfStudyDto>>;