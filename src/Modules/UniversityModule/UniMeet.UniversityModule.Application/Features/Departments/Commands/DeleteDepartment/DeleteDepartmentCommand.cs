using MediatR;

namespace UniMeet.UniversityModule.Application.Features.Departments.Commands.DeleteDepartment;
public record DeleteDepartmentCommand(int UniversityId, int DepartmentId) : IRequest;