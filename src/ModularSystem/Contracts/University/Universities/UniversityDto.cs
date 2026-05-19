using ModularSystem.Contracts.University.AllowedEmailDomains;
using ModularSystem.Contracts.University.Departments;

namespace ModularSystem.Contracts.University.Universities;

public class UniversityDto
{
    public required int Id { get; set; }
    public required string Name { get; set; } = null!;
    public required string Country { get; set; } = null!;
    public required string Voivodeship { get; set; } = null!;
    public required string City { get; set; } = null!;
    public required string Address { get; set; } = null!;
    
    public required ICollection<DepartmentDto> Departments { get; set; } = new List<DepartmentDto>();
    public required ICollection<AllowedEmailDomainDto> AllowedEmailDomains { get; set; } = new List<AllowedEmailDomainDto>();
}