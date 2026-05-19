using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.University.Departments.DeleteDepartment;

public record DeleteDepartmentCommand(int DepartmentId) : ICommand;