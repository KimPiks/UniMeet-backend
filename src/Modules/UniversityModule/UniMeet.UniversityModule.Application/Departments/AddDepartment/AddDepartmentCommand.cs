using UniMeet.Shared.Abstractions;

namespace UniMeet.UniversityModule.Application.Departments.AddDepartment;

public record AddDepartmentCommand(int UniversityId, string DepartmentName) : IRequest;