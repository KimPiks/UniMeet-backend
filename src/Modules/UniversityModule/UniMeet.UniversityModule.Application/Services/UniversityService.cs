using UniMeet.UniversityModule.Application.DTOs;
using UniMeet.UniversityModule.Application.Interfaces;
using UniMeet.UniversityModule.Application.Mappers;
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

    public async Task<UniversityDto?> GetUniversityByIdAsync(int universityId)
    {
        var university = await _universityRepository.GetByIdAsync(universityId);
        if (university == null)
        {
            return null;
        }

        return university.ToDto();
    }

    public async Task<IEnumerable<UniversityDto>> GetAllUniversitiesAsync()
    {
        var universities = await _universityRepository.GetAllAsync();
        return universities.Select(university => university.ToDto());
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
    
    public async Task UpdateUniversityAsync(int universityId, string? name, string? country, string? voivodeship, string? city, string? address)
    {
        var university = await _universityRepository.GetByIdAsync(universityId);
        if (university == null)
        {
            throw new ArgumentException("University not found");
        }

        if (!string.IsNullOrEmpty(name))
        {
            university.Rename(name);
        }
        
        if (!string.IsNullOrEmpty(country))
        {
            university.ChangeCountry(country);
        }
        
        if (!string.IsNullOrEmpty(voivodeship))
        {
            university.ChangeVoivodeship(voivodeship);
        }
        
        if (!string.IsNullOrEmpty(city))
        {
            university.ChangeCity(city);
        }
        
        if (!string.IsNullOrEmpty(address))
        {
            university.ChangeAddress(address);
        }
        
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

    public async Task<IEnumerable<DepartmentDto>> GetDepartmentsByUniversityIdAsync(int universityId)
    {
        var university = await _universityRepository.GetByIdAsync(universityId);
        if (university == null)
        {
            throw new ArgumentException("University not found");
        }
        
        return university.Departments.Select(department => department.ToDto());
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
    
    public async Task UpdateDepartmentAsync(int universityId, int departmentId, string? newDepartmentName)
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
        
        if (!string.IsNullOrEmpty(newDepartmentName))
        {
            university.RenameDepartment(department.Name, newDepartmentName);
        }
        
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

    public async Task<IEnumerable<AllowedEmailDomainDto>> GetAllowedEmailDomainsByUniversityIdAsync(int universityId)
    {
        var university = await _universityRepository.GetByIdAsync(universityId);
        if (university == null)
        {
            throw new ArgumentException("University not found");
        }
        
        return university.AllowedEmailDomains.Select(allowedDomain => allowedDomain.ToDto());
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
    
    public async Task UpdateAllowedEmailDomainAsync(int universityId, int domainId, string? newDomain)
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

        if (!string.IsNullOrEmpty(newDomain))
        {
            university.ChangeAllowedEmailDomain(domain.Domain, newDomain);
        }
        
        await _universityRepository.SaveChangesAsync();
    }

    public async Task AddFieldOfStudyAsync(int universityId, int departmentId, string fieldOfStudyName)
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
        
        university.AddFieldOfStudyToDepartment(department.Name, fieldOfStudyName);
        await _universityRepository.SaveChangesAsync();
    }

    public async Task<IEnumerable<FieldOfStudyDto>> GetFieldsOfStudyByDepartmentIdAsync(int universityId, int departmentId)
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
        
        return department.FieldsOfStudy.Select(fos => new FieldOfStudyDto()
        {
            Id = fos.Id,
            Name = fos.Name
        });
    }

    public async Task DeleteFieldOfStudyAsync(int universityId, int departmentId, int fieldOfStudyId)
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
        
        var fieldOfStudy = department.FieldsOfStudy.FirstOrDefault(fos => fos.Id == fieldOfStudyId);
        if (fieldOfStudy == null)
        {
            throw new ArgumentException("Field of study not found");
        }
        
        university.RemoveFieldOfStudyFromDepartment(department.Name, fieldOfStudy.Name);
        await _universityRepository.SaveChangesAsync();
    }
    
    public async Task UpdateFieldOfStudyAsync(int universityId, int departmentId, int fieldOfStudyId, string? newFieldOfStudyName)
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
        
        var fieldOfStudy = department.FieldsOfStudy.FirstOrDefault(fos => fos.Id == fieldOfStudyId);
        if (fieldOfStudy == null)
        {
            throw new ArgumentException("Field of study not found");
        }
        
        if (!string.IsNullOrEmpty(newFieldOfStudyName))
        {
            university.RenameFieldOfStudyInDepartment(department.Name, fieldOfStudy.Name, newFieldOfStudyName);
        }
        
        await _universityRepository.SaveChangesAsync();
    }
}