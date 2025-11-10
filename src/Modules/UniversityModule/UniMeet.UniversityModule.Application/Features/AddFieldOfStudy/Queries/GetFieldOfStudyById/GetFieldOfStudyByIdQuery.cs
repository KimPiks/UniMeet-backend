using UniMeet.Shared.Abstractions;
using UniMeet.UniversityModule.Application.DTOs;

namespace UniMeet.UniversityModule.Application.Features.FieldsOfStudy.Queries.GetFieldOfStudyById;

public record GetFieldOfStudyByIdQuery(int UniversityId, int DepartmentId, int FieldOfStudyId) : IRequest<FieldOfStudyDto?>;