using UniMeet.UniversityModule.Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;
using UniMeet.Shared.Abstractions;

namespace UniMeet.UniversityModule.Application.Features.Departments.Commands.AddDepartment;

public class AddDepartmentCommandHandler : IRequestHandler<AddDepartmentCommand>
{
    private readonly IUniversityRepository _universityRepository;

    public AddDepartmentCommandHandler(IUniversityRepository universityRepository)
    {
        _universityRepository = universityRepository;
    }

    public async Task HandleAsync(AddDepartmentCommand request, CancellationToken cancellationToken)
    {
        var university = await _universityRepository.GetByIdAsync(request.UniversityId);
        if (university == null)
        {
            throw new ArgumentException("University not found");
        }

        university.AddDepartment(request.DepartmentName, request.UniversityId);
        await _universityRepository.SaveChangesAsync(cancellationToken);
    }
}