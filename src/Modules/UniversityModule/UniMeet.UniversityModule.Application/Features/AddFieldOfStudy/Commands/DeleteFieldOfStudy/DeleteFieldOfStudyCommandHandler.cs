using UniMeet.UniversityModule.Domain.Repositories;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UniMeet.Shared.Abstractions;

namespace UniMeet.UniversityModule.Application.Features.FieldsOfStudy.Commands.DeleteFieldOfStudy;

public class DeleteFieldOfStudyCommandHandler : IRequestHandler<DeleteFieldOfStudyCommand>
{
    private readonly IUniversityRepository _universityRepository;

    public DeleteFieldOfStudyCommandHandler(IUniversityRepository universityRepository)
    {
        _universityRepository = universityRepository;
    }

    public async Task HandleAsync(DeleteFieldOfStudyCommand request, CancellationToken cancellationToken)
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
        
        var fieldOfStudy = department.FieldsOfStudy.FirstOrDefault(fos => fos.Id == request.FieldOfStudyId);
        if (fieldOfStudy == null)
        {
            throw new ArgumentException("Field of study not found");
        }
        
        university.RemoveFieldOfStudyFromDepartment(department.Name, fieldOfStudy.Name);
        await _universityRepository.SaveChangesAsync(cancellationToken);
    }
}