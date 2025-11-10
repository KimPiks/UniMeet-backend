namespace UniMeet.UniversityModule.Domain.Universities;

public class University
{
    public int Id { get; set; }
    public string Name { get; private set; } = null!;
    public string Country { get; private set; } = null!;
    public string Voivodeship { get; private set; } = null!;
    public string City { get; private set; } = null!;
    public string Address { get; private set; } = null!;
    
    public ICollection<Department> Departments { get; private set; } = new List<Department>();
    public ICollection<AllowedEmailDomain> AllowedEmailDomains { get; private set; } = new List<AllowedEmailDomain>();
    
    private University() { }
    
    public University(string name, string country, string voivodeship, string city, string address)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("University name cannot be null or empty.", nameof(name));
        
        if (string.IsNullOrWhiteSpace(country)) 
            throw new ArgumentException("Country cannot be null or empty.", nameof(country));
        
        if (string.IsNullOrWhiteSpace(voivodeship)) 
            throw new ArgumentException("Voivodeship cannot be null or empty.", nameof(voivodeship));
        
        if (string.IsNullOrWhiteSpace(city)) 
            throw new ArgumentException("City cannot be null or empty.", nameof(city));
        
        if (string.IsNullOrWhiteSpace(address)) 
            throw new ArgumentException("Address cannot be null or empty.", nameof(address));
        
        Name = name;
        Country = country;
        Voivodeship = voivodeship;
        City = city;
        Address = address;
    }
    
    public void Rename(string newName)
    {
        Name = newName;
    }
    
    public void ChangeAddress(string newAddress)
    {
        Address = newAddress;
    }
    
    public void ChangeCity(string newCity)
    {
        City = newCity;
    }
    
    public void ChangeVoivodeship(string newVoivodeship)
    {
        Voivodeship = newVoivodeship;
    }
    
    public void ChangeCountry(string newCountry)
    {
        Country = newCountry;
    }
    
    public FieldOfStudy? GetFieldOfStudyById(int fieldOfStudyId)
    {
        return Departments.Select(department => 
            department.FieldsOfStudy.FirstOrDefault(f => f.Id == fieldOfStudyId))
            .OfType<FieldOfStudy>()
            .FirstOrDefault();
    }
    
    public void AddDepartment(string name)
    {
        if (Departments.Any(d => d.Name == name))
            throw new InvalidOperationException($"Department with name '{name}' already exists in this university.");
        
        var department = new Department(name, Id);
        Departments.Add(department);
    }
    
    public void AddAllowedEmailDomain(string domain)
    {
        if (AllowedEmailDomains.Any(d => d.Domain == domain))
            throw new InvalidOperationException($"Email domain '{domain}' is already allowed for this university.");
        
        var allowedEmailDomain = new AllowedEmailDomain(domain, Id);
        AllowedEmailDomains.Add(allowedEmailDomain);
    }
    
    public void RemoveAllowedEmailDomain(int allowedDomainId)
    {
        var allowedEmailDomain = AllowedEmailDomains.FirstOrDefault(d => d.Id == allowedDomainId);
        if (allowedEmailDomain == null)
            throw new InvalidOperationException($"Email domain with id '{allowedEmailDomain}' is not found in this university.");
        
        AllowedEmailDomains.Remove(allowedEmailDomain);
    }
    
    public void ChangeAllowedEmailDomain(int allowedDomainId, string newDomain)
    {
        var allowedEmailDomain = AllowedEmailDomains.FirstOrDefault(d => d.Id == allowedDomainId);
        if (allowedEmailDomain == null)
            throw new InvalidOperationException($"Email domain with id '{allowedEmailDomain}' is not found in this university.");
        
        if (AllowedEmailDomains.Any(d => d.Domain == newDomain))
            throw new InvalidOperationException($"Email domain '{newDomain}' is already allowed for this university.");
        
        allowedEmailDomain.ChangeDomain(newDomain);
    }
    
    public void RemoveDepartment(int departmentId)
    {
        var department = Departments.FirstOrDefault(d => d.Id == departmentId);
        if (department == null)
            throw new InvalidOperationException($"Department with id '{departmentId}' is not found in this university.");
        
        Departments.Remove(department);
    }
    
    public void RenameDepartment(int departmentId, string newName)
    {
        var department = Departments.FirstOrDefault(d => d.Id == departmentId);
        if (department == null)
            throw new InvalidOperationException($"Department with id '{departmentId}' is not found in this university.");

        if (Departments.Any(d => d.Name == newName))
            throw new InvalidOperationException($"Department with name '{newName}' already exists in this university.");
        
        department.Rename(newName);
    }
    
    public void AddFieldOfStudy(int departmentId, string fieldOfStudyName)
    {
        var department = Departments.FirstOrDefault(d => d.Id == departmentId);
        if (department == null)
            throw new InvalidOperationException($"Department with id '{departmentId}' is not found in this university.");
        
        department.AddFieldOfStudy(fieldOfStudyName);
    }
    
    public void RemoveFieldOfStudy(int fieldOfStudyId)
    {
        var department = Departments.FirstOrDefault(d => d.FieldsOfStudy.Any(f => f.Id == fieldOfStudyId));
        if (department == null)
            throw new InvalidOperationException($"Field of study with id '{fieldOfStudyId}' is not found in this university.");
        
        department.RemoveFieldOfStudy(fieldOfStudyId);
    }
    
    public void RenameFieldOfStudy(int fieldOfStudyId, string newFieldOfStudyName)
    {
        var department = Departments.FirstOrDefault(d => d.FieldsOfStudy.Any(f => f.Id == fieldOfStudyId));
        if (department == null)
            throw new InvalidOperationException($"Field of study with id '{fieldOfStudyId}' is not found in this university.");

        department.RenameFieldOfStudy(fieldOfStudyId, newFieldOfStudyName);
    }
}