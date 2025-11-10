using UniMeet.Shared.Abstractions;
using UniMeet.UniversityModule.Domain.Universities;

namespace UniMeet.UniversityModule.Application.FieldsOfStudy.UpdateFieldOfStudy;

public class UpdateFieldOfStudyCommandHandler(IUniversityRepository universityRepository)
    : ICommandHandler<UpdateFieldOfStudyCommand>
{
    public async Task HandleAsync(UpdateFieldOfStudyCommand request, CancellationToken cancellationToken)
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
        
        if (!string.IsNullOrEmpty(request.NewFieldOfStudyName))
        {
            university.RenameFieldOfStudyInDepartment(department.Name, fieldOfStudy.Name, request.NewFieldOfStudyName);
        }
        
        await universityRepository.SaveChangesAsync(cancellationToken);
    }
}