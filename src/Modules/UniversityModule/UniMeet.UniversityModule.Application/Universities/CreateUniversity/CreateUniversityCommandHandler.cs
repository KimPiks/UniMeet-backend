using UniMeet.Shared.Abstractions;
using UniMeet.UniversityModule.Domain.Universities;

namespace UniMeet.UniversityModule.Application.Universities.CreateUniversity;

public class CreateUniversityCommandHandler(IUniversityRepository universityRepository)
    : ICommandHandler<CreateUniversityCommand>
{
    public async Task HandleAsync(CreateUniversityCommand request, CancellationToken cancellationToken)
    {
        request.Validate();
        
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