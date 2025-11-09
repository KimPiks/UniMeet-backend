using MediatR;
using UniMeet.UniversityModule.Application.DTOs;
using UniMeet.UniversityModule.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace UniMeet.UniversityModule.Application.Features.FieldsOfStudy.Queries.GetFieldsOfStudyByDepartmentId;

public class GetFieldsOfStudyByDepartmentIdQueryHandler : IRequestHandler<GetFieldsOfStudyByDepartmentIdQuery, IEnumerable<FieldOfStudyDto>>
{
    private readonly IUniversityRepository _universityRepository;

    public GetFieldsOfStudyByDepartmentIdQueryHandler(IUniversityRepository universityRepository)
    {
        _universityRepository = universityRepository;
    }

    public async Task<IEnumerable<FieldOfStudyDto>> Handle(GetFieldsOfStudyByDepartmentIdQuery request, CancellationToken cancellationToken)
    {
        var university = await _universityRepository.GetByIdAsync(request.UniversityId);
        if (university == null)
        {
            throw new ArgumentException("University not found");
        }
        
        var department = university.Departments.FirstOrDefault(d => d.Id == request.DepartmentId);
        if (department == null)
        {
            throw new ArgumentException("Department not found");
        }
        
        return department.FieldsOfStudy.Select(fos => new FieldOfStudyDto()
        {
            Id = fos.Id,
            Name = fos.Name
        }).ToList(); 
    }
}