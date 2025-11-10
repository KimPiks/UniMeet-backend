using UniMeet.Shared.Abstractions;
using UniMeet.UniversityModule.Domain.Universities;

namespace UniMeet.UniversityModule.Application.Departments.AddDepartment;

public class AddDepartmentCommandHandler(IUniversityRepository universityRepository)
    : ICommandHandler<AddDepartmentCommand>
{
    public async Task HandleAsync(AddDepartmentCommand request, CancellationToken cancellationToken)
    {
        request.Validate();
        
        var university = await universityRepository.GetByIdAsync(request.UniversityId, cancellationToken);
        if (university == null)
            throw new KeyNotFoundException("University not found");

        university.AddDepartment(request.DepartmentName);
        await universityRepository.SaveChangesAsync(cancellationToken);
    }
}