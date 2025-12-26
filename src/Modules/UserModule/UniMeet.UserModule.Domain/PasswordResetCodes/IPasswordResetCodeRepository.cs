namespace UniMeet.UserModule.Domain.PasswordResetCodes;

public interface IPasswordResetCodeRepository
{
    Task<PasswordResetCode?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PasswordResetCode?> GetByCodeAsync(Guid code, CancellationToken cancellationToken = default);
    Task<IEnumerable<PasswordResetCode>> GetAllAsync(int offset, int limit, CancellationToken cancellationToken = default);
    Task AddAsync(PasswordResetCode code, CancellationToken cancellationToken = default);
    void Delete(PasswordResetCode code);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}