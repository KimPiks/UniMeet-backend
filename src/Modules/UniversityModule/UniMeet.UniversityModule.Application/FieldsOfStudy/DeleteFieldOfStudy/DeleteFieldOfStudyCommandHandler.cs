using UniMeet.Shared.Abstractions;
using UniMeet.UniversityModule.Domain.Repositories;

namespace UniMeet.UniversityModule.Application.FieldsOfStudy.DeleteFieldOfStudy;

public class DeleteFieldOfStudyCommandHandler(IUniversityRepository universityRepository)
    : IRequestHandler<DeleteFieldOfStudyCommand>
{
    public async Task HandleAsync(DeleteFieldOfStudyCommand request, CancellationToken cancellationToken)
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
            throw new ArgumentException("Field of study not found");
        }
        
        university.RemoveFieldOfStudyFromDepartment(department.Name, fieldOfStudy.Name);
        await universityRepository.SaveChangesAsync(cancellationToken);
    }
}