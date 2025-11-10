using UniMeet.UniversityModule.Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;
using UniMeet.Shared.Abstractions;

namespace UniMeet.UniversityModule.Application.Universities.Commands.UpdateUniversity;

public class UpdateUniversityCommandHandler : IRequestHandler<UpdateUniversityCommand>
{
    private readonly IUniversityRepository _universityRepository;

    public UpdateUniversityCommandHandler(IUniversityRepository universityRepository)
    {
        _universityRepository = universityRepository;
    }

    public async Task HandleAsync(UpdateUniversityCommand request, CancellationToken cancellationToken)
    {

        var university = await _universityRepository.GetByIdAsync(request.UniversityId);
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
        
        await _universityRepository.SaveChangesAsync(cancellationToken);
    }
}