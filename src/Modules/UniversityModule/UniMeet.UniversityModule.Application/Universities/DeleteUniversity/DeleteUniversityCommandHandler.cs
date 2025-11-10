using UniMeet.Shared.Abstractions;
using UniMeet.UniversityModule.Domain.Universities;

namespace UniMeet.UniversityModule.Application.Universities.DeleteUniversity;

public class DeleteUniversityCommandHandler(IUniversityRepository universityRepository)
    : ICommandHandler<DeleteUniversityCommand>
{
    public async Task HandleAsync(DeleteUniversityCommand request, CancellationToken cancellationToken)
    {
        request.Validate();
        
        var university = await universityRepository.GetByIdAsync(request.UniversityId, cancellationToken);
        if (university == null)
            throw new KeyNotFoundException("University not found");
        
        universityRepository.Delete(university);
        await universityRepository.SaveChangesAsync(cancellationToken);
    }
}