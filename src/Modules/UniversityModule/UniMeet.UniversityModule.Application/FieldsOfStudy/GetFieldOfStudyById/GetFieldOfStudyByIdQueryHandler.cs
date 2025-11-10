using UniMeet.Shared.Abstractions;
using UniMeet.UniversityModule.Domain.Universities;

namespace UniMeet.UniversityModule.Application.FieldsOfStudy.GetFieldOfStudyById;

public class GetFieldOfStudyByIdQueryHandler(IUniversityRepository universityRepository)
    : IQueryHandler<GetFieldOfStudyByIdQuery, FieldOfStudyDto?>
{
    public async Task<FieldOfStudyDto?> HandleAsync(GetFieldOfStudyByIdQuery request, CancellationToken cancellationToken)
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