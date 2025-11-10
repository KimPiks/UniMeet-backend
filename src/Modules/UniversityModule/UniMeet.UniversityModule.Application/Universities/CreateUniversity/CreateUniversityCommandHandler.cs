using UniMeet.Shared.Abstractions;
using UniMeet.UniversityModule.Domain.Aggregates.UniversityAggregate;
using UniMeet.UniversityModule.Domain.Universities;

namespace UniMeet.UniversityModule.Application.Universities.CreateUniversity;

public class CreateUniversityCommandHandler(IUniversityRepository universityRepository)
    : IRequestHandler<CreateUniversityCommand>
{
    public async Task HandleAsync(CreateUniversityCommand request, CancellationToken cancellationToken)
    {
        var university = new University(
            request.Name,
            request.Country,
            request.Voivodeship,
            request.City,
            request.Address
        );

        await universityRepository.AddAsync(university, cancellationToken);
        await universityRepository.SaveChangesAsync(cancellationToken);
    }
}