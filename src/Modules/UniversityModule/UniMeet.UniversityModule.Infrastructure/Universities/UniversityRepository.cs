using Microsoft.EntityFrameworkCore;
using UniMeet.UniversityModule.Domain.Universities;

namespace UniMeet.UniversityModule.Infrastructure.Universities;

public class UniversityRepository(UniversityContext context) : IUniversityRepository
{
    public async Task<University?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await context.Universities
            .Include(u => u.AllowedEmailDomains)
            .Include(u => u.Departments)
            .ThenInclude(d => d.FieldsOfStudy)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<University?> GetByAllowedDomainIdAsync(int allowedDomainId, CancellationToken cancellationToken = default)
    {
        return await context.Universities
            .Include(u => u.AllowedEmailDomains)
            .Include(u => u.Departments)
            .ThenInclude(d => d.FieldsOfStudy)
            .FirstOrDefaultAsync(u => u.AllowedEmailDomains.Any(d => d.Id == allowedDomainId), cancellationToken);
    }

    public async Task<University?> GetByAllowedEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await context.Universities
            .Include(u => u.AllowedEmailDomains)
            .Include(u => u.Departments)
            .ThenInclude(d => d.FieldsOfStudy)
            .FirstOrDefaultAsync(u => u.AllowedEmailDomains.Any(d => d.Domain == email.ToLower()), cancellationToken);
    }

    public async Task<University?> GetByDepartmentIdAsync(int departmentId,
        CancellationToken cancellationToken = default)
    {
        return await context.Universities
            .Include(u => u.AllowedEmailDomains)
            .Include(u => u.Departments)
            .ThenInclude(d => d.FieldsOfStudy)
            .FirstOrDefaultAsync(u => u.Departments.Any(d => d.Id == departmentId), cancellationToken);
    }
    
    public async Task<University?> GetByFieldOfStudyIdAsync(int fieldOfStudyId,
        CancellationToken cancellationToken = default)
    {
        return await context.Universities
            .Include(u => u.AllowedEmailDomains)
            .Include(u => u.Departments)
            .ThenInclude(d => d.FieldsOfStudy)
            .FirstOrDefaultAsync(u => u.Departments.Any(d => d.FieldsOfStudy.Any(fos => fos.Id == fieldOfStudyId)), cancellationToken);
    }

    public async Task<IEnumerable<University>> GetAllAsync(int offset, int limit, CancellationToken cancellationToken = default)
    {
        return await context.Universities
            .Include(u => u.AllowedEmailDomains)
            .Include(u => u.Departments)
            .ThenInclude(d => d.FieldsOfStudy)
            .Skip(offset)
            .Take(limit)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(University university, CancellationToken cancellationToken = default)
    {
        await context.Universities.AddAsync(university, cancellationToken);
    }

    public void Delete(University university)
    {
        context.Universities.Remove(university);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}