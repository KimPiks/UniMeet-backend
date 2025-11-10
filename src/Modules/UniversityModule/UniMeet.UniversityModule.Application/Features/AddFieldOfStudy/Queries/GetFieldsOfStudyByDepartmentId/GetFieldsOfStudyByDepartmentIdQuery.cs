using UniMeet.UniversityModule.Application.DTOs;
using System.Collections.Generic;
using UniMeet.Shared.Abstractions;

namespace UniMeet.UniversityModule.Application.Features.FieldsOfStudy.Queries.GetFieldsOfStudyByDepartmentId;

public record GetFieldsOfStudyByDepartmentIdQuery(int UniversityId, int DepartmentId) : IRequest<IEnumerable<FieldOfStudyDto>>;