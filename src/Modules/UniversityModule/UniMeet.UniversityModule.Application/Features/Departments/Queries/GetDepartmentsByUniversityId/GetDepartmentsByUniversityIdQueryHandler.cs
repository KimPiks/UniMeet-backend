using UniMeet.UniversityModule.Application.DTOs;
using UniMeet.UniversityModule.Application.Mappers; 
using UniMeet.UniversityModule.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UniMeet.Shared.Abstractions;

namespace UniMeet.UniversityModule.Application.Features.Departments.Queries.GetDepartmentsByUniversityId;

public class GetDepartmentsByUniversityIdQueryHandler : IRequestHandler<GetDepartmentsByUniversityIdQuery, IEnumerable<DepartmentDto>>
{
    private readonly IUniversityRepository _universityRepository;

    public GetDepartmentsByUniversityIdQueryHandler(IUniversityRepository universityRepository)
    {
        _universityRepository = universityRepository;
    }

    public async Task<IEnumerable<DepartmentDto>> HandleAsync(GetDepartmentsByUniversityIdQuery request, CancellationToken cancellationToken)
    {
        var university = await _universityRepository.GetByIdAsync(request.UniversityId);
        if (university == null)
        {
            
            throw new ArgumentException("University not found");
        }
        return university.Departments.Select(department => department.ToDto()).ToList();
    }
}