namespace UniMeet.UniversityModule.Domain.Universities;

public interface IUniversityRepository
{
    Task<University?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<University?> GetByAllowedDomainIdAsync(int allowedDomainId, CancellationToken cancellationToken = default);
    Task<University?> GetByAllowedEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<University?> GetByDepartmentIdAsync(int departmentId, CancellationToken cancellationToken = default);
    Task<University?> GetByFieldOfStudyIdAsync(int fieldOfStudyId, CancellationToken cancellationToken = default);
    Task<IEnumerable<University>> GetAllAsync(int offset, int limit, CancellationToken cancellationToken = default);
    Task AddAsync(University university, CancellationToken cancellationToken = default);
    void Delete(University university);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}