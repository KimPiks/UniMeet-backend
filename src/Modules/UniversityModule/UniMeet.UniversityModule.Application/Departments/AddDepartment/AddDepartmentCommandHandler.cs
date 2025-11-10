using UniMeet.Shared.Abstractions;
using UniMeet.UniversityModule.Domain.Repositories;

namespace UniMeet.UniversityModule.Application.Departments.AddDepartment;

public class AddDepartmentCommandHandler(IUniversityRepository universityRepository)
    : IRequestHandler<AddDepartmentCommand>
{
    public async Task HandleAsync(AddDepartmentCommand request, CancellationToken cancellationToken)
    {
        var university = await universityRepository.GetByIdAsync(request.UniversityId, cancellationToken);
        if (university == null)
        {
            throw new ArgumentException("University not found");
        }

        university.AddDepartment(request.DepartmentName, request.UniversityId);
        await universityRepository.SaveChangesAsync(cancellationToken);
    }
}