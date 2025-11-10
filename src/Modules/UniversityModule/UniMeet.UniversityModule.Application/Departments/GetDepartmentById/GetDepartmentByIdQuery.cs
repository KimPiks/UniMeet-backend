using UniMeet.Shared.Abstractions;

namespace UniMeet.UniversityModule.Application.Departments.GetDepartmentById;

public record GetDepartmentByIdQuery(int UniversityId, int DepartmentId) : IQuery<DepartmentDto?>;