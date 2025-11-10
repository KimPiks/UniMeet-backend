using UniMeet.UniversityModule.Domain.Aggregates.UniversityAggregate;

namespace UniMeet.UniversityModule.Domain.Universities;

public interface IUniversityRepository
{
    Task<University?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<University?> GetByDomainAsync(string domain, CancellationToken cancellationToken = default);
    Task<IEnumerable<University>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(University university, CancellationToken cancellationToken = default);
    void Delete(University university);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}