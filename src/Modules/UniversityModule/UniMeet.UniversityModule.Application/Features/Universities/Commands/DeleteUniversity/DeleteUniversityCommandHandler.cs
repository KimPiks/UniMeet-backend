using MediatR;
using UniMeet.UniversityModule.Domain.Repositories;
using System; // Potrzebne dla ArgumentException
using System.Threading;
using System.Threading.Tasks;

namespace UniMeet.UniversityModule.Application.Universities.Commands.DeleteUniversity;

/// <summary>
/// Handler (klasa obsługująca) dla komendy DeleteUniversityCommand.
/// </summary>
public class DeleteUniversityCommandHandler : IRequestHandler<DeleteUniversityCommand>
{
    private readonly IUniversityRepository _universityRepository;

    public DeleteUniversityCommandHandler(IUniversityRepository universityRepository)
    {
        _universityRepository = universityRepository;
    }

    public async Task Handle(DeleteUniversityCommand request, CancellationToken cancellationToken)
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