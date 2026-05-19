using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.University.Departments.UpdateDepartment;

public record UpdateDepartmentCommand(int DepartmentId, string NewDepartmentName) : ICommand;