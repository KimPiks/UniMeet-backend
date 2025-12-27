using UniMeet.Shared.Abstractions;
using UniMeet.UserModule.Domain.PasswordResetCodes;
using UniMeet.UserModule.Domain.PasswordResetCodes.Exceptions;
using UniMeet.UserModule.Domain.Services;

namespace UniMeet.UserModule.Application.PasswordResetCodes.ResetPassword;

public class ResetPasswordCommandHandler(IPasswordResetCodeRepository passwordResetCodeRepository,
    IPasswordHasher passwordHasher) : ICommandHandler<ResetPasswordCommand>
{
    public async Task HandleAsync(ResetPasswordCommand request, CancellationToken cancellationToken = default)
    {
        request.Validate();
        
        var passwordResetCode = await passwordResetCodeRepository.GetByCodeAsync(request.Code, cancellationToken);
        if (passwordResetCode == null)
            throw new CodeNotFoundException(request.Code);

        if (passwordResetCode.IsExpired())
            throw new CodeExpiredException(request.Code);

        var passwordHash = passwordHasher.Hash(request.NewPassword);
        passwordResetCode.User.UpdatePassword(passwordHash);
        passwordResetCodeRepository.Delete(passwordResetCode);
        await passwordResetCodeRepository.SaveChangesAsync(cancellationToken);
    }
}