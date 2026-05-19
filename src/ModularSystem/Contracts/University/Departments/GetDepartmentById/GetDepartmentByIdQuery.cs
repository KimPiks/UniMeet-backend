using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.University.Departments.GetDepartmentById;

public record GetDepartmentByIdQuery(int DepartmentId) : IQuery<DepartmentDto?>;