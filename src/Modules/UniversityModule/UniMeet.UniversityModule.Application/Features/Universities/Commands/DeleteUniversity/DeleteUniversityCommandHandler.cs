using UniMeet.UniversityModule.Domain.Repositories;
using System; 
using System.Threading;
using System.Threading.Tasks;
using UniMeet.Shared.Abstractions;

namespace UniMeet.UniversityModule.Application.Universities.Commands.DeleteUniversity;

public class DeleteUniversityCommandHandler : IRequestHandler<DeleteUniversityCommand>
{
    private readonly IUniversityRepository _universityRepository;

    public DeleteUniversityCommandHandler(IUniversityRepository universityRepository)
    {
        _universityRepository = universityRepository;
    }

    public async Task HandleAsync(DeleteUniversityCommand request, CancellationToken cancellationToken)
    {
        var university = await _universityRepository.GetByIdAsync(request.UniversityId);
        
        if (university == null)
        {
            throw new ArgumentException("University not found");
        }
        
        _universityRepository.Delete(university);
        await _universityRepository.SaveChangesAsync(cancellationToken);
    }
}