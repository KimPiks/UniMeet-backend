using UniMeet.UniversityModule.Domain.Repositories;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UniMeet.Shared.Abstractions;

namespace UniMeet.UniversityModule.Application.Features.FieldsOfStudy.Commands.AddFieldOfStudy;

public class AddFieldOfStudyCommandHandler : IRequestHandler<AddFieldOfStudyCommand>
{
    private readonly IUniversityRepository _universityRepository;

    public AddFieldOfStudyCommandHandler(IUniversityRepository universityRepository)
    {
        _universityRepository = universityRepository;
    }

    public async Task HandleAsync(AddFieldOfStudyCommand request, CancellationToken cancellationToken = default)
    {
        var university = await _universityRepository.GetByIdAsync(request.UniversityId);
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
        await _universityRepository.SaveChangesAsync(cancellationToken);
    }
}