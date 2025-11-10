using UniMeet.UniversityModule.Application.AllowedEmailDomains;
using UniMeet.UniversityModule.Application.Departments;
using UniMeet.UniversityModule.Domain.Aggregates.UniversityAggregate;

namespace UniMeet.UniversityModule.Application.Universities;

public static class UniversityMapper
{
    public static UniversityDto ToDto(this University university)
    {
        return new UniversityDto
        {
            Id = university.Id,
            Name = university.Name,
            Country = university.Country,
            Voivodeship = university.Voivodeship,
            City = university.City,
            Address = university.Address,
            
            Departments = university.Departments.Select(department => department.ToDto())
                .ToList(),
            AllowedEmailDomains = university.AllowedEmailDomains.Select(domain => domain.ToDto())
                .ToList(),
        };
    }
}