using UniMeet.Shared.Abstractions;
using UniMeet.UniversityModule.Domain.Universities;

namespace UniMeet.UniversityModule.Application.Departments.GetDepartmentsByUniversityId;

public class GetDepartmentsByUniversityIdQueryHandler(IUniversityRepository universityRepository)
    : IQueryHandler<GetDepartmentsByUniversityIdQuery, IEnumerable<DepartmentDto>>
{
    public async Task<IEnumerable<DepartmentDto>> HandleAsync(GetDepartmentsByUniversityIdQuery request, CancellationToken cancellationToken)
    {
        request.Validate();
        
        var university = await universityRepository.GetByIdAsync(request.UniversityId, cancellationToken);
        if (university == null)
            throw new KeyNotFoundException("University not found");
        
        return university.Departments.Select(department => department.ToDto()).ToList();
    }
}