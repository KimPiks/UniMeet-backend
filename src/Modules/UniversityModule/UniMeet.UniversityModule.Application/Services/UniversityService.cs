using UniMeet.UniversityModule.Application.Interfaces;
using UniMeet.UniversityModule.Domain.Aggregates.UniversityAggregate;
using UniMeet.UniversityModule.Domain.Repositories;

namespace UniMeet.UniversityModule.Application.Services;

public class UniversityService : IUniversityService
{
    private readonly IUniversityRepository _universityRepository;
    
    public UniversityService(IUniversityRepository universityRepository)
    {
        _universityRepository = universityRepository;
    }

    public async Task<University?> GetUniversityByIdAsync(int universityId)
    {
        var university = await _universityRepository.GetByIdAsync(universityId);
        return university;
    }

    public async Task<IEnumerable<University>> GetAllUniversitiesAsync()
    {
        var universities = await _universityRepository.GetAllAsync();
        return universities;
    }

    public async Task CreateUniversityAsync(string name, string country, string voivodeship, string city, string address)
    {
        var university = new University(name, country, voivodeship, city, address);
        await _universityRepository.AddAsync(university);
        await _universityRepository.SaveChangesAsync();
    }

    public async Task DeleteUniversityAsync(int universityId)
    {
        var university = await _universityRepository.GetByIdAsync(universityId);
        if (university == null)
        {
            throw new ArgumentException("University not found");
        }
        
        _universityRepository.Delete(university);
        await _universityRepository.SaveChangesAsync();
    }

    public async Task AddDepartmentAsync(int universityId, string departmentName)
    {
        var university = await _universityRepository.GetByIdAsync(universityId);
        if (university == null)
        {
            throw new ArgumentException("University not found");
        }
        
        university.AddDepartment(departmentName, universityId);
        await _universityRepository.SaveChangesAsync();
    }

    public async Task<IEnumerable<Department>> GetDepartmentsByUniversityIdAsync(int universityId)
    {
        var university = await _universityRepository.GetByIdAsync(universityId);
        if (university == null)
        {
            throw new ArgumentException("University not found");
        }
        
        return university.Departments;
    }

    public async Task DeleteDepartmentAsync(int universityId, int departmentId)
    {
        var university = await _universityRepository.GetByIdAsync(universityId);
        if (university == null)
        {
            throw new ArgumentException("University not found");
        }
        
        var department = university.Departments.FirstOrDefault(d => d.Id == departmentId);
        if (department == null)
        {
            throw new ArgumentException("Department not found");
        }
        
        university.RemoveDepartment(department.Name);
        await _universityRepository.SaveChangesAsync();
    }

    public async Task AddAllowedEmailDomainAsync(int universityId, string domain)
    {
        var university = await _universityRepository.GetByIdAsync(universityId);
        if (university == null)
        {
            throw new ArgumentException("University not found");
        }
        
        university.AddAllowedEmailDomain(domain, universityId);
        await _universityRepository.SaveChangesAsync();
    }

    public async Task<IEnumerable<AllowedEmailDomain>> GetAllowedEmailDomainsByUniversityIdAsync(int universityId)
    {
        var university = await _universityRepository.GetByIdAsync(universityId);
        if (university == null)
        {
            throw new ArgumentException("University not found");
        }
        
        return university.AllowedEmailDomains;
    }

    public async Task DeleteAllowedEmailDomainAsync(int universityId, int domainId)
    {
        var university = await _universityRepository.GetByIdAsync(universityId);
        if (university == null)
        {
            throw new ArgumentException("University not found");
        }
        
        var domain = university.AllowedEmailDomains.FirstOrDefault(d => d.Id == domainId);
        if (domain == null)
        {
            throw new ArgumentException("Allowed email domain not found");
        }
        
        university.RemoveAllowedEmailDomain(domain.Domain);
        await _universityRepository.SaveChangesAsync();
    }
}