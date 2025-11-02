using Microsoft.EntityFrameworkCore;
using UniMeet.UniversityModule.Domain.Aggregates.UniversityAggregate;
using UniMeet.UniversityModule.Domain.Repositories;

namespace UniMeet.UniversityModule.Infrastructure.Repositories;

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

    public async Task<University?> GetByDomainAsync(string domain, CancellationToken cancellationToken = default)
    {
        return await context.Universities
            .Include(u => u.AllowedEmailDomains)
            .Include(u => u.Departments)
            .ThenInclude(d => d.FieldsOfStudy)
            .FirstOrDefaultAsync(u => u.AllowedEmailDomains.Any(d => d.Domain == domain), cancellationToken);
    }

    public async Task<IEnumerable<University>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Universities
            .Include(u => u.AllowedEmailDomains)
            .Include(u => u.Departments)
            .ThenInclude(d => d.FieldsOfStudy)
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