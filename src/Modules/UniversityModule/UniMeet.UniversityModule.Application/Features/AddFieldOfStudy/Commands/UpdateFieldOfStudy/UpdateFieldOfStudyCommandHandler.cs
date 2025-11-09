using MediatR;
using UniMeet.UniversityModule.Domain.Repositories;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace UniMeet.UniversityModule.Application.Features.FieldsOfStudy.Commands.UpdateFieldOfStudy;

public class UpdateFieldOfStudyCommandHandler : IRequestHandler<UpdateFieldOfStudyCommand>
{
    private readonly IUniversityRepository _universityRepository;

    public UpdateFieldOfStudyCommandHandler(IUniversityRepository universityRepository)
    {
        _universityRepository = universityRepository;
    }

    public async Task Handle(UpdateFieldOfStudyCommand request, CancellationToken cancellationToken)
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
        
        if (!string.IsNullOrEmpty(request.NewFieldOfStudyName))
        {
            university.RenameFieldOfStudyInDepartment(department.Name, fieldOfStudy.Name, request.NewFieldOfStudyName);
        }
        
        await _universityRepository.SaveChangesAsync(cancellationToken);
    }
}