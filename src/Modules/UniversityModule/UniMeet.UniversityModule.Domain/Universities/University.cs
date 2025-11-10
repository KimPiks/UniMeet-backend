using UniMeet.UniversityModule.Domain.Universities.Exceptions;

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
            throw new InvalidUniversityNameException(name);
        
        if (string.IsNullOrWhiteSpace(country)) 
            throw new InvalidCountryNameException(country);

        if (string.IsNullOrWhiteSpace(voivodeship))
            throw new InvalidVoivodeshipNameException(voivodeship);
        
        if (string.IsNullOrWhiteSpace(city)) 
            throw new InvalidCityNameException(city);
        
        if (string.IsNullOrWhiteSpace(address)) 
            throw new InvalidAddressException(address);
        
        Name = name;
        Country = country;
        Voivodeship = voivodeship;
        City = city;
        Address = address;
    }
    
    public void Rename(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new InvalidUniversityNameException(newName);
        
        Name = newName;
    }
    
    public void ChangeAddress(string newAddress)
    {
        if (string.IsNullOrWhiteSpace(newAddress)) 
            throw new InvalidAddressException(newAddress);
        
        Address = newAddress;
    }
    
    public void ChangeCity(string newCity)
    {
        if (string.IsNullOrWhiteSpace(newCity)) 
            throw new InvalidCityNameException(newCity);
        
        City = newCity;
    }
    
    public void ChangeVoivodeship(string newVoivodeship)
    {
        if (string.IsNullOrWhiteSpace(newVoivodeship))
            throw new InvalidVoivodeshipNameException(newVoivodeship);
        
        Voivodeship = newVoivodeship;
    }
    
    public void ChangeCountry(string newCountry)
    {
        if (string.IsNullOrWhiteSpace(newCountry)) 
            throw new InvalidCountryNameException(newCountry);
        
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
        if (string.IsNullOrWhiteSpace(name))
            throw new InvalidDepartmentNameException(name);
        
        if (Departments.Any(d => d.Name == name))
            throw new DepartmentAlreadyExistsException(name);
        
        var department = new Department(name, Id);
        Departments.Add(department);
    }
    
    public void AddAllowedEmailDomain(string domain)
    {
        if (string.IsNullOrWhiteSpace(domain))
            throw new InvalidAllowedEmailDomainNameException(domain);
        
        if (AllowedEmailDomains.Any(d => d.Domain == domain))
            throw new AllowedDomainAlreadyExistsException(domain);
        
        var allowedEmailDomain = new AllowedEmailDomain(domain, Id);
        AllowedEmailDomains.Add(allowedEmailDomain);
    }
    
    public void RemoveAllowedEmailDomain(int allowedDomainId)
    {
        var allowedEmailDomain = AllowedEmailDomains.FirstOrDefault(d => d.Id == allowedDomainId);
        if (allowedEmailDomain == null)
            throw new AllowedDomainNotFoundException(allowedDomainId);
        
        AllowedEmailDomains.Remove(allowedEmailDomain);
    }
    
    public void ChangeAllowedEmailDomain(int allowedDomainId, string newDomain)
    {
        if (string.IsNullOrWhiteSpace(newDomain))
            throw new InvalidAllowedEmailDomainNameException(newDomain);
        
        var allowedEmailDomain = AllowedEmailDomains.FirstOrDefault(d => d.Id == allowedDomainId);
        if (allowedEmailDomain == null)
            throw new AllowedDomainNotFoundException(allowedDomainId);
        
        if (AllowedEmailDomains.Any(d => d.Domain == newDomain))
            throw new AllowedDomainAlreadyExistsException(newDomain);
        
        allowedEmailDomain.ChangeDomain(newDomain);
    }
    
    public void RemoveDepartment(int departmentId)
    {
        var department = Departments.FirstOrDefault(d => d.Id == departmentId);
        if (department == null)
            throw new DepartmentNotFoundException(departmentId);
        
        Departments.Remove(department);
    }
    
    public void RenameDepartment(int departmentId, string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new InvalidDepartmentNameException(newName);
        
        var department = Departments.FirstOrDefault(d => d.Id == departmentId);
        if (department == null)
            throw new DepartmentNotFoundException(departmentId);

        if (Departments.Any(d => d.Name == newName))
            throw new DepartmentAlreadyExistsException(newName);
        
        department.Rename(newName);
    }
    
    public void AddFieldOfStudy(int departmentId, string fieldOfStudyName)
    {
        var department = Departments.FirstOrDefault(d => d.Id == departmentId);
        if (department == null)
            throw new DepartmentNotFoundException(departmentId);
        
        department.AddFieldOfStudy(fieldOfStudyName);
    }
    
    public void RemoveFieldOfStudy(int fieldOfStudyId)
    {
        var department = Departments.FirstOrDefault(d => d.FieldsOfStudy.Any(f => f.Id == fieldOfStudyId));
        if (department == null)
            throw new FieldOfStudyNotFoundException(fieldOfStudyId);
        
        department.RemoveFieldOfStudy(fieldOfStudyId);
    }
    
    public void RenameFieldOfStudy(int fieldOfStudyId, string newFieldOfStudyName)
    {
        if (string.IsNullOrWhiteSpace(newFieldOfStudyName))
            throw new InvalidFieldOfStudyNameException(newFieldOfStudyName);
        
        var department = Departments.FirstOrDefault(d => d.FieldsOfStudy.Any(f => f.Id == fieldOfStudyId));
        if (department == null)
            throw new FieldOfStudyNotFoundException(fieldOfStudyId);
        
        department.RenameFieldOfStudy(fieldOfStudyId, newFieldOfStudyName);
    }
}