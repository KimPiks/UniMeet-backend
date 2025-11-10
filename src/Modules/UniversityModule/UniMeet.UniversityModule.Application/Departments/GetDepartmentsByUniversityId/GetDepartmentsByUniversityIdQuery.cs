using UniMeet.Shared.Abstractions;
using UniMeet.UniversityModule.Application.DTOs;

namespace UniMeet.UniversityModule.Application.Departments.GetDepartmentsByUniversityId;

public record GetDepartmentsByUniversityIdQuery(int UniversityId) : IRequest<IEnumerable<DepartmentDto>>;