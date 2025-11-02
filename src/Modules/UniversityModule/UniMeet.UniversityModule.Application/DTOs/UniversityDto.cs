namespace UniMeet.UniversityModule.Application.DTOs;

public class UniversityDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Country { get; set; } = null!;
    public string Voivodeship { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Address { get; set; } = null!;
    
    public ICollection<DepartmentDto> Departments { get; set; } = new List<DepartmentDto>();
    public ICollection<AllowedEmailDomainDto> AllowedEmailDomains { get; set; } = new List<AllowedEmailDomainDto>();
}