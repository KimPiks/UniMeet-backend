using UniMeet.Shared.Abstractions;
using UniMeet.UniversityModule.Domain.Universities;

namespace UniMeet.UniversityModule.Application.Universities.UpdateUniversity;

public class UpdateUniversityCommandHandler(IUniversityRepository universityRepository)
    : ICommandHandler<UpdateUniversityCommand>
{
    public async Task HandleAsync(UpdateUniversityCommand request, CancellationToken cancellationToken)
    {
        request.Validate();

        var university = await universityRepository.GetByIdAsync(request.UniversityId, cancellationToken);
        if (university == null)
            throw new KeyNotFoundException("University not found");

        university.Rename(request.Name);
        university.ChangeCountry(request.Country);
        university.ChangeVoivodeship(request.Voivodeship);
        university.ChangeCity(request.City);
        university.ChangeAddress(request.Address);
        
        await universityRepository.SaveChangesAsync(cancellationToken);
    }
}