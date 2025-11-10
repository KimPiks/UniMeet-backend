using UniMeet.Shared.Abstractions;

namespace UniMeet.UniversityModule.Application.FieldsOfStudy.GetFieldOfStudyById;

public record GetFieldOfStudyByIdQuery(int UniversityId, int DepartmentId, int FieldOfStudyId) : IQuery<FieldOfStudyDto?>;