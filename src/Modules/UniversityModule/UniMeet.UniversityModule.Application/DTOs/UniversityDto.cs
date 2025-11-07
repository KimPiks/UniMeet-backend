namespace UniMeet.UniversityModule.Application.DTOs;

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