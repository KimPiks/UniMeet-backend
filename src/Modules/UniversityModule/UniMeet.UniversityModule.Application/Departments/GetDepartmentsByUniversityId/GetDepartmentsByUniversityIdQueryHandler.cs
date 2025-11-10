using UniMeet.Shared.Abstractions;
using UniMeet.UniversityModule.Application.DTOs;
using UniMeet.UniversityModule.Application.Mappers;
using UniMeet.UniversityModule.Domain.Repositories;

namespace UniMeet.UniversityModule.Application.Departments.GetDepartmentsByUniversityId;

public class GetDepartmentsByUniversityIdQueryHandler(IUniversityRepository universityRepository)
    : IRequestHandler<GetDepartmentsByUniversityIdQuery, IEnumerable<DepartmentDto>>
{
    public async Task<IEnumerable<DepartmentDto>> HandleAsync(GetDepartmentsByUniversityIdQuery request, CancellationToken cancellationToken)
    {
        var university = await universityRepository.GetByIdAsync(request.UniversityId, cancellationToken);
        if (university == null)
        {
            
            throw new ArgumentException("University not found");
        }
        return university.Departments.Select(department => department.ToDto()).ToList();
    }
}