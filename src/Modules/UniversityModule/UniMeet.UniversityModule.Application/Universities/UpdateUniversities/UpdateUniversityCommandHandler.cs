using UniMeet.Shared.Abstractions;
using UniMeet.UniversityModule.Domain.Universities;

namespace UniMeet.UniversityModule.Application.Universities.UpdateUniversities;

public class UpdateUniversityCommandHandler(IUniversityRepository universityRepository)
    : ICommandHandler<UpdateUniversityCommand>
{
    public async Task HandleAsync(UpdateUniversityCommand request, CancellationToken cancellationToken)
    {

        var university = await universityRepository.GetByIdAsync(request.UniversityId, cancellationToken);
        if (university == null)
        {
            throw new ArgumentException("University not found");
        }

        if (!string.IsNullOrEmpty(request.Name))
        {
            university.Rename(request.Name);
        }
        
        if (!string.IsNullOrEmpty(request.Country))
        {
            university.ChangeCountry(request.Country);
        }
        
        if (!string.IsNullOrEmpty(request.Voivodeship))
        {
            university.ChangeVoivodeship(request.Voivodeship);
        }
        
        if (!string.IsNullOrEmpty(request.City))
        {
            university.ChangeCity(request.City);
        }
        
        if (!string.IsNullOrEmpty(request.Address))
        {
            university.ChangeAddress(request.Address);
        }
        
        await universityRepository.SaveChangesAsync(cancellationToken);
    }
}