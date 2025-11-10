using UniMeet.Shared.Abstractions;
using UniMeet.UniversityModule.Domain.Universities;

namespace UniMeet.UniversityModule.Application.Departments.GetDepartmentById;

public class GetDepartmentByIdQueryHandler(IUniversityRepository universityRepository)
    : IQueryHandler<GetDepartmentByIdQuery, DepartmentDto?>
{
    public async Task<DepartmentDto?> HandleAsync(GetDepartmentByIdQuery request, CancellationToken cancellationToken)
    {
        var university = await universityRepository.GetByDepartmentIdAsync(request.DepartmentId, cancellationToken);
        if (university == null)
            throw new ArgumentException("University not found");
        
        var department = university.Departments.FirstOrDefault(d => d.Id == request.DepartmentId);

        return department?.ToDto();
    }
}