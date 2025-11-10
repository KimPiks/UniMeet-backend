using UniMeet.Shared.Abstractions;
using UniMeet.UniversityModule.Domain.Universities;

namespace UniMeet.UniversityModule.Application.FieldsOfStudy.GetFieldsOfStudyByDepartmentId;

public class GetFieldsOfStudyByDepartmentIdQueryHandler(IUniversityRepository universityRepository)
    : IRequestHandler<GetFieldsOfStudyByDepartmentIdQuery, IEnumerable<FieldOfStudyDto>>
{
    public async Task<IEnumerable<FieldOfStudyDto>> HandleAsync(GetFieldsOfStudyByDepartmentIdQuery request, CancellationToken cancellationToken)
    {
        var university = await universityRepository.GetByIdAsync(request.UniversityId, cancellationToken);
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