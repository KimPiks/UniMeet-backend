using MediatR;
using UniMeet.UniversityModule.Application.DTOs;
using UniMeet.UniversityModule.Domain.Repositories;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace UniMeet.UniversityModule.Application.Features.FieldsOfStudy.Queries.GetFieldOfStudyById;

public class GetFieldOfStudyByIdQueryHandler : IRequestHandler<GetFieldOfStudyByIdQuery, FieldOfStudyDto?>
{
    private readonly IUniversityRepository _universityRepository;

    public GetFieldOfStudyByIdQueryHandler(IUniversityRepository universityRepository)
    {
        _universityRepository = universityRepository;
    }

    public async Task<FieldOfStudyDto?> Handle(GetFieldOfStudyByIdQuery request, CancellationToken cancellationToken)
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

        var fieldOfStudy = department.FieldsOfStudy.FirstOrDefault(fos => fos.Id == request.FieldOfStudyId);
        
        if (fieldOfStudy == null)
        {
            return null;
        }

        return new FieldOfStudyDto
        {
            Id = fieldOfStudy.Id,
            Name = fieldOfStudy.Name
        };
    }
}