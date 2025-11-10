using UniMeet.Shared.Abstractions;
using UniMeet.UniversityModule.Domain.Repositories;

namespace UniMeet.UniversityModule.Application.FieldsOfStudy.AddFieldOfStudy;

public class AddFieldOfStudyCommandHandler(IUniversityRepository universityRepository)
    : IRequestHandler<AddFieldOfStudyCommand>
{
    public async Task HandleAsync(AddFieldOfStudyCommand request, CancellationToken cancellationToken = default)
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

        university.AddFieldOfStudyToDepartment(department.Name, request.FieldOfStudyName);
        await universityRepository.SaveChangesAsync(cancellationToken);
    }
}