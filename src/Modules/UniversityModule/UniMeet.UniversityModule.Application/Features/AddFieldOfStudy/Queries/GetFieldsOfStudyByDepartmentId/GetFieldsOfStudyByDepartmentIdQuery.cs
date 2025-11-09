using MediatR;
using UniMeet.UniversityModule.Application.DTOs;
using System.Collections.Generic;

namespace UniMeet.UniversityModule.Application.Features.FieldsOfStudy.Queries.GetFieldsOfStudyByDepartmentId;

public record GetFieldsOfStudyByDepartmentIdQuery(int UniversityId, int DepartmentId) : IRequest<IEnumerable<FieldOfStudyDto>>;