using UniMeet.Shared.Abstractions;

namespace UniMeet.UniversityModule.Application.Features.Departments.Commands.AddDepartment;

public record AddDepartmentCommand(int UniversityId, string DepartmentName) : IRequest;