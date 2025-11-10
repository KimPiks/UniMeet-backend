using UniMeet.Shared.Abstractions;
using UniMeet.UniversityModule.Domain.Universities;

namespace UniMeet.UniversityModule.Application.FieldsOfStudy.GetFieldsOfStudyByDepartmentId;

public class GetFieldsOfStudyByDepartmentIdQueryHandler(IUniversityRepository universityRepository)
    : IQueryHandler<GetFieldsOfStudyByDepartmentIdQuery, IEnumerable<FieldOfStudyDto>>
{
    public async Task<IEnumerable<FieldOfStudyDto>> HandleAsync(GetFieldsOfStudyByDepartmentIdQuery request, CancellationToken cancellationToken)
    {
        request.Validate();
        
        var university = await universityRepository.GetByDepartmentIdAsync(request.DepartmentId, cancellationToken);
        if (university == null)
            throw new KeyNotFoundException("University not found");
        
        var department = university.Departments.FirstOrDefault(d => d.Id == request.DepartmentId);
        if (department == null)
            throw new KeyNotFoundException("Department not found");
        
        return department.FieldsOfStudy.Select(fos => new FieldOfStudyDto()
        {
            Id = fos.Id,
            Name = fos.Name
        }).ToList(); 
    }
}