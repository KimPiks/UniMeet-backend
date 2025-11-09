using MediatR;
using UniMeet.UniversityModule.Domain.Aggregates.UniversityAggregate;
using UniMeet.UniversityModule.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace UniMeet.UniversityModule.Application.Universities.Commands.CreateUniversity;


public class CreateUniversityCommandHandler : IRequestHandler<CreateUniversityCommand>
{
    private readonly IUniversityRepository _universityRepository;

    public CreateUniversityCommandHandler(IUniversityRepository universityRepository)
    {
        _universityRepository = universityRepository;
    }

    public async Task Handle(CreateUniversityCommand request, CancellationToken cancellationToken)
    {
        var university = new University(
            request.Name,
            request.Country,
            request.Voivodeship,
            request.City,
            request.Address
        );

        await _universityRepository.AddAsync(university);
        await _universityRepository.SaveChangesAsync(cancellationToken);
    }
}