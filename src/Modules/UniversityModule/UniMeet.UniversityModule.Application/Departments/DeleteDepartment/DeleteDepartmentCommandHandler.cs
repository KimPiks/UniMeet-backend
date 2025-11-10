using UniMeet.Shared.Abstractions;
using UniMeet.UniversityModule.Domain.Universities;

namespace UniMeet.UniversityModule.Application.Departments.DeleteDepartment;

public class DeleteDepartmentCommandHandler(IUniversityRepository universityRepository)
    : ICommandHandler<DeleteDepartmentCommand>
{
    public async Task HandleAsync(DeleteDepartmentCommand request, CancellationToken cancellationToken)
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
        
        university.RemoveDepartment(department.Name);
        await universityRepository.SaveChangesAsync(cancellationToken);
    }
}