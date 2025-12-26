using UniMeet.Shared.Abstractions;
using UniMeet.UserModule.Domain.PasswordResetCodes;

namespace UniMeet.UserModule.Application.PasswordResetCodes.CheckIfResetPasswordCodeExists;

public class CheckIfResetPasswordCodeExistsQueryHandler(IPasswordResetCodeRepository passwordResetCodeRepository) : IQueryHandler<CheckIfResetPasswordCodeExistsQuery, bool>
{
    public async Task<bool> HandleAsync(CheckIfResetPasswordCodeExistsQuery request, CancellationToken cancellationToken = default)
    {
        var exists = await passwordResetCodeRepository.GetByCodeAsync(request.Code, cancellationToken) != null;
        return exists;
    }
}