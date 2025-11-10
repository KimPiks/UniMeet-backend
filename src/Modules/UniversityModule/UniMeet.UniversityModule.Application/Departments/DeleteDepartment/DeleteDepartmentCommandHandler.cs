using UniMeet.Shared.Abstractions;
using UniMeet.UniversityModule.Domain.Universities;

namespace UniMeet.UniversityModule.Application.Departments.DeleteDepartment;

public class DeleteDepartmentCommandHandler(IUniversityRepository universityRepository)
    : ICommandHandler<DeleteDepartmentCommand>
{
    public async Task HandleAsync(DeleteDepartmentCommand request, CancellationToken cancellationToken)
    {
        request.Validate();
        
        var university = await universityRepository.GetByDepartmentIdAsync(request.DepartmentId, cancellationToken);
        if (university == null)
            throw new KeyNotFoundException("University not found for the given department");
        
        var department = university.Departments.FirstOrDefault(d => d.Id == request.DepartmentId);
        if (department == null) 
            throw new KeyNotFoundException("Department not found");
        
        university.RemoveDepartment(request.DepartmentId);
        await universityRepository.SaveChangesAsync(cancellationToken);
    }
}