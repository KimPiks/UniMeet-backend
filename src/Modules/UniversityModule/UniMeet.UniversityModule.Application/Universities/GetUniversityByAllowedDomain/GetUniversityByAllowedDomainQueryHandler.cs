using ModularSystem.Contracts.University;
using UniMeet.Shared.Abstractions;
using UniMeet.Shared.Exceptions;
using UniMeet.UniversityModule.Domain.Universities;

namespace UniMeet.UniversityModule.Application.Universities.GetUniversityByAllowedDomain;

public class GetUniversityByAllowedDomainQueryHandler(IUniversityRepository universityRepository)
    : IQueryHandler<GetUniversityByAllowedDomainQuery, UniversityLookupDto?>
{
    public async Task<UniversityLookupDto?> HandleAsync(
        GetUniversityByAllowedDomainQuery request,
        CancellationToken cancellationToken = default)
    {
        Validate(request);

        var university = await universityRepository.GetByAllowedEmailAsync(request.Domain, cancellationToken);
        return university is null ? null : new UniversityLookupDto(university.Id, university.Name);
    }

    private static void Validate(GetUniversityByAllowedDomainQuery request)
    {
        var errors = new List<string>();

        if (!request.Domain.Contains("."))
        {
            errors.Add("Invalid email domain.");
        }

        if (errors.Count > 0)
        {
            throw new ValidationException(errors);
        }
    }
}
