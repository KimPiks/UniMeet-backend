using UniMeet.Shared.Abstractions;
using UniMeet.UniversityModule.Domain.Universities;

namespace UniMeet.UniversityModule.Application.FieldsOfStudy.AddFieldOfStudy;

public class AddFieldOfStudyCommandHandler(IUniversityRepository universityRepository)
    : ICommandHandler<AddFieldOfStudyCommand>
{
    public async Task HandleAsync(AddFieldOfStudyCommand request, CancellationToken cancellationToken = default)
    {
        var university = await universityRepository.GetByDepartmentIdAsync(request.DepartmentId, cancellationToken);
        if (university == null)
            throw new ArgumentException("University not found");
        
        var department = university.Departments.FirstOrDefault(d => d.Id == request.DepartmentId);
        if (department == null)
            throw new ArgumentException("Department not found");

        university.AddFieldOfStudy(request.DepartmentId, request.FieldOfStudyName);
        await universityRepository.SaveChangesAsync(cancellationToken);
    }
}