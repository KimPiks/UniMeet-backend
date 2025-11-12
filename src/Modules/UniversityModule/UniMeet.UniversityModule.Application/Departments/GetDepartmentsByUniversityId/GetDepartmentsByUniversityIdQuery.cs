using UniMeet.Shared.Abstractions;

namespace UniMeet.UniversityModule.Application.Departments.GetDepartmentsByUniversityId;

public record GetDepartmentsByUniversityIdQuery(int UniversityId, int Offset, int Limit) : IQuery<IEnumerable<DepartmentDto>>;