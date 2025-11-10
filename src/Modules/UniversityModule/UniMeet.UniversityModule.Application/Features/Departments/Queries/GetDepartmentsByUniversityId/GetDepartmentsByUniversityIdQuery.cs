using UniMeet.UniversityModule.Application.DTOs;
using System.Collections.Generic;
using UniMeet.Shared.Abstractions;

namespace UniMeet.UniversityModule.Application.Features.Departments.Queries.GetDepartmentsByUniversityId;

public record GetDepartmentsByUniversityIdQuery(int UniversityId) : IRequest<IEnumerable<DepartmentDto>>;