using UniMeet.UniversityModule.Domain.Universities;

namespace UniMeet.UniversityModule.Application.AllowedEmailDomains;

public static class AllowedEmailDomainDtoMapper
{
    public static AllowedEmailDomainDto ToDto(this AllowedEmailDomain allowedEmailDomain)
    {
        return new AllowedEmailDomainDto
        {
            Id = allowedEmailDomain.Id,
            Domain = allowedEmailDomain.Domain
        };
    }
}