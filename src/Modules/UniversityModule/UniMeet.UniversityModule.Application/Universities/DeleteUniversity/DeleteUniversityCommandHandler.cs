using UniMeet.Shared.Abstractions;
using UniMeet.UniversityModule.Domain.Universities;

namespace UniMeet.UniversityModule.Application.Universities.DeleteUniversity;

public class DeleteUniversityCommandHandler(IUniversityRepository universityRepository)
    : IRequestHandler<DeleteUniversityCommand>
{
    public async Task HandleAsync(DeleteUniversityCommand request, CancellationToken cancellationToken)
    {
        var university = await universityRepository.GetByIdAsync(request.UniversityId, cancellationToken);
        
        if (university == null)
        {
            throw new ArgumentException("University not found");
        }
        
        universityRepository.Delete(university);
        await universityRepository.SaveChangesAsync(cancellationToken);
    }
}