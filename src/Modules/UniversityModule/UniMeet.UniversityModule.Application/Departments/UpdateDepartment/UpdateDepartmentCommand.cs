using UniMeet.Shared.Abstractions;

namespace UniMeet.UniversityModule.Application.Departments.UpdateDepartment;

public record UpdateDepartmentCommand(int UniversityId, int DepartmentId, string? NewDepartmentName) : ICommand;