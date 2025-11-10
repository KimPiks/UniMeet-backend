using UniMeet.Shared.Abstractions;
using UniMeet.UniversityModule.Domain.Universities;

namespace UniMeet.UniversityModule.Application.Departments.UpdateDepartment;

public class UpdateDepartmentCommandHandler(IUniversityRepository universityRepository)
    : ICommandHandler<UpdateDepartmentCommand>
{
    public async Task HandleAsync(UpdateDepartmentCommand request, CancellationToken cancellationToken)
    {
        var university = await universityRepository.GetByDepartmentIdAsync(request.DepartmentId, cancellationToken);
        if (university == null)
            throw new ArgumentException("University not found");
        
        var department = university.Departments.FirstOrDefault(d => d.Id == request.DepartmentId);
        if (department == null)
            throw new ArgumentException("Department not found");
        
        university.RenameDepartment(request.DepartmentId, request.NewDepartmentName);
        await universityRepository.SaveChangesAsync(cancellationToken);
    }
}