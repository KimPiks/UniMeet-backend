using MediatR;

namespace UniMeet.UniversityModule.Application.Features.Departments.Commands.UpdateDepartment;

public record UpdateDepartmentCommand(int UniversityId, int DepartmentId, string? NewDepartmentName) : IRequest;