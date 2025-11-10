using UniMeet.Shared.Abstractions;

namespace UniMeet.UniversityModule.Application.Departments.GetDepartmentsByUniversityId;

public record GetDepartmentsByUniversityIdQuery(int UniversityId) : IQuery<IEnumerable<DepartmentDto>>;