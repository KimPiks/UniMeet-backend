using UniMeet.Shared.Abstractions;
using UniMeet.UniversityModule.Domain.Universities;

namespace UniMeet.UniversityModule.Application.FieldsOfStudy.DeleteFieldOfStudy;

public class DeleteFieldOfStudyCommandHandler(IUniversityRepository universityRepository)
    : ICommandHandler<DeleteFieldOfStudyCommand>
{
    public async Task HandleAsync(DeleteFieldOfStudyCommand request, CancellationToken cancellationToken)
    {
        request.Validate();
        
        var university = await universityRepository.GetByFieldOfStudyIdAsync(request.FieldOfStudyId, cancellationToken);
        if (university == null)
            throw new KeyNotFoundException("University not found");
        
        var fieldOfStudy = university.GetFieldOfStudyById(request.FieldOfStudyId);
        if (fieldOfStudy == null)
            throw new KeyNotFoundException("Field of study not found");
        
        university.RemoveFieldOfStudy(request.FieldOfStudyId);
        await universityRepository.SaveChangesAsync(cancellationToken);
    }
}