using UniMeet.Shared.Abstractions;

namespace UniMeet.UniversityModule.Application.Departments.UpdateDepartment;

public record UpdateDepartmentCommand(int DepartmentId, string NewDepartmentName) : ICommand;