using UniMeet.Shared.Abstractions;

namespace UniMeet.UniversityModule.Application.Departments.DeleteDepartment;

public record DeleteDepartmentCommand(int DepartmentId) : ICommand;