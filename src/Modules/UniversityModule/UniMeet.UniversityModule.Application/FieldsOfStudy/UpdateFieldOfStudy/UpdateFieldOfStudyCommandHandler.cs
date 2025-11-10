using UniMeet.Shared.Abstractions;
using UniMeet.UniversityModule.Domain.Universities;

namespace UniMeet.UniversityModule.Application.FieldsOfStudy.UpdateFieldOfStudy;

public class UpdateFieldOfStudyCommandHandler(IUniversityRepository universityRepository)
    : ICommandHandler<UpdateFieldOfStudyCommand>
{
    public async Task HandleAsync(UpdateFieldOfStudyCommand request, CancellationToken cancellationToken)
    {
        var university = await universityRepository.GetByFieldOfStudyIdAsync(request.FieldOfStudyId, cancellationToken);
        if (university == null)
            throw new ArgumentException("University not found");

        var fieldOfStudy = university.GetFieldOfStudyById(request.FieldOfStudyId);
        if (fieldOfStudy == null)
            throw new ArgumentException("Field of study not found");
        
        university.RenameFieldOfStudy(request.FieldOfStudyId, request.NewFieldOfStudyName);
        await universityRepository.SaveChangesAsync(cancellationToken);
    }
}