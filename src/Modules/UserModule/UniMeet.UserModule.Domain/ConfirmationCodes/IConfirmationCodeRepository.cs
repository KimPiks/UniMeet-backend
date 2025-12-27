namespace UniMeet.UserModule.Domain.ConfirmationCodes;

public interface IConfirmationCodeRepository
{
    Task<ConfirmationCode?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ConfirmationCode?> GetByCodeAsync(Guid code, CancellationToken cancellationToken = default);
    Task<IEnumerable<ConfirmationCode>> GetAllAsync(int offset, int limit, CancellationToken cancellationToken = default);
    Task AddAsync(ConfirmationCode code, CancellationToken cancellationToken = default);
    void Delete(ConfirmationCode code);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}