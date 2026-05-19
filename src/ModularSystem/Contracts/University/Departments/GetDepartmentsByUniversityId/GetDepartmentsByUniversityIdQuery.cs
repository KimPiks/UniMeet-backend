using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.University.Departments.GetDepartmentsByUniversityId;

public record GetDepartmentsByUniversityIdQuery(int UniversityId, int Offset, int Limit) : IQuery<IEnumerable<DepartmentDto>>;