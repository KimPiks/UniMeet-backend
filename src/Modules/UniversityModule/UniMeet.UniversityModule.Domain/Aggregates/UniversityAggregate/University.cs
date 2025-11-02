namespace UniMeet.UniversityModule.Domain.Aggregates.UniversityAggregate;

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
    
    public Department AddDepartment(string name, int universityId)
    {
        if (Departments.Any(d => d.Name == name))
        {
            throw new InvalidOperationException($"Department with name '{name}' already exists in this university.");
        }
        
        var department = new Department(name, universityId);
        Departments.Add(department);
        return department;
    }
    
    public AllowedEmailDomain AddAllowedEmailDomain(string domain, int universityId)
    {
        if (AllowedEmailDomains.Any(d => d.Domain == domain))
        {
            throw new InvalidOperationException($"Email domain '{domain}' is already allowed for this university.");
        }
        
        var allowedEmailDomain = new AllowedEmailDomain(domain, universityId);
        AllowedEmailDomains.Add(allowedEmailDomain);
        return allowedEmailDomain;
    }
    
    public void RemoveAllowedEmailDomain(string domain)
    {
        var allowedEmailDomain = AllowedEmailDomains.FirstOrDefault(d => d.Domain == domain);
        if (allowedEmailDomain == null)
        {
            throw new InvalidOperationException($"Email domain '{domain}' is not found in this university.");
        }
        
        AllowedEmailDomains.Remove(allowedEmailDomain);
    }
    
    public void ChangeDomain(string domain, string newDomain)
    {
        var allowedEmailDomain = AllowedEmailDomains.FirstOrDefault(d => d.Domain == domain);
        if (allowedEmailDomain == null)
        {
            throw new InvalidOperationException($"Email domain '{domain}' is not found in this university.");
        }
        if (AllowedEmailDomains.Any(d => d.Domain == newDomain))
        {
            throw new InvalidOperationException($"Email domain '{newDomain}' is already allowed for this university.");
        }
        
        allowedEmailDomain.ChangeDomain(newDomain);
    }
    
    public void RemoveDepartment(string name)
    {
        var department = Departments.FirstOrDefault(d => d.Name == name);
        if (department == null)
        {
            throw new InvalidOperationException($"Department with name '{name}' is not found in this university.");
        }
        
        Departments.Remove(department);
    }
    
    public void RenameDepartment(string name, string newName)
    {
        var department = Departments.FirstOrDefault(d => d.Name == name);
        if (department == null)
        {
            throw new InvalidOperationException($"Department with name '{name}' is not found in this university.");
        }
        if (Departments.Any(d => d.Name == newName))
        {
            throw new InvalidOperationException($"Department with name '{newName}' already exists in this university.");
        }
        
        department.Rename(newName);
    }
    
    public FieldOfStudy AddFieldOfStudyToDepartment(string departmentName, string fieldOfStudyName)
    {
        var department = Departments.FirstOrDefault(d => d.Name == departmentName);
        if (department == null)
        {
            throw new InvalidOperationException($"Department with name '{departmentName}' is not found in this university.");
        }
        
        var fieldOfStudy = department.AddFieldOfStudy(fieldOfStudyName, department.Id);
        return fieldOfStudy;
    }
    
    public void RemoveFieldOfStudyFromDepartment(string departmentName, string fieldOfStudyName)
    {
        var department = Departments.FirstOrDefault(d => d.Name == departmentName);
        if (department == null)
        {
            throw new InvalidOperationException($"Department with name '{departmentName}' is not found in this university.");
        }
        
        department.RemoveFieldOfStudy(fieldOfStudyName);
    }
    
    public void RenameFieldOfStudyInDepartment(string departmentName, string currentFieldOfStudyName, string newFieldOfStudyName)
    {
        var department = Departments.FirstOrDefault(d => d.Name == departmentName);
        if (department == null)
        {
            throw new InvalidOperationException($"Department with name '{departmentName}' is not found in this university.");
        }
        
        department.RenameFieldOfStudy(currentFieldOfStudyName, newFieldOfStudyName);
    }
}