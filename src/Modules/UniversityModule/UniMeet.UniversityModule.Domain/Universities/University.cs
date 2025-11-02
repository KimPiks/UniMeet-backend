namespace UniMeet.UniversityModule.Domain.Universities;

public class University
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Country { get; set; } = null!;
    public string Voivodeship { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Address { get; set; } = null!;
    
    public ICollection<Department> Departments { get; set; } = new List<Department>();
    public ICollection<AllowedEmailDomain> AllowedEmailDomains { get; set; } = new List<AllowedEmailDomain>();
    
    private University() { }
    
    public University(string name, string country, string voivodeship, string city, string address)
    {
        Name = name;
        Country = country;
        Voivodeship = voivodeship;
        City = city;
        Address = address;
    }
}