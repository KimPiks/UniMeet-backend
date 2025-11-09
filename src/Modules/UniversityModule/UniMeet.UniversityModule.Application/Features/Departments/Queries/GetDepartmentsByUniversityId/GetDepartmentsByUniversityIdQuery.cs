using MediatR;
using UniMeet.UniversityModule.Application.DTOs;
using System.Collections.Generic;

namespace UniMeet.UniversityModule.Application.Features.Departments.Queries.GetDepartmentsByUniversityId;

public record GetDepartmentsByUniversityIdQuery(int UniversityId) : IRequest<IEnumerable<DepartmentDto>>;