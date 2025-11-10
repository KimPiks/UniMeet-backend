using UniMeet.Shared.Abstractions;
using UniMeet.UniversityModule.Domain.Universities;

namespace UniMeet.UniversityModule.Application.Departments.UpdateDepartment;

public class UpdateDepartmentCommandHandler(IUniversityRepository universityRepository)
    : ICommandHandler<UpdateDepartmentCommand>
{
    public async Task HandleAsync(UpdateDepartmentCommand request, CancellationToken cancellationToken)
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
        
        if (!string.IsNullOrEmpty(request.NewDepartmentName))
        {
            university.RenameDepartment(department.Name, request.NewDepartmentName);
        }
        
        await universityRepository.SaveChangesAsync(cancellationToken);
    }
}