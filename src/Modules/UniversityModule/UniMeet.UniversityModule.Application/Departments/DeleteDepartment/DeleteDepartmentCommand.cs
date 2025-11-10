using UniMeet.Shared.Abstractions;

namespace UniMeet.UniversityModule.Application.Departments.DeleteDepartment;

public record DeleteDepartmentCommand(int UniversityId, int DepartmentId) : IRequest;