using UniMeet.Shared.Abstractions;
using UniMeet.UniversityModule.Application.DTOs;

namespace UniMeet.UniversityModule.Application.Features.Departments.Queries.GetDepartmentById;

public record GetDepartmentByIdQuery(int UniversityId, int DepartmentId) : IRequest<DepartmentDto?>;