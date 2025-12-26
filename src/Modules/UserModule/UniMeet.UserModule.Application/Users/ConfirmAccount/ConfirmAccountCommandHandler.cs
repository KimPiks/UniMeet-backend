using UniMeet.Shared.Abstractions;
using UniMeet.UserModule.Domain.ConfirmationCodes;
using UniMeet.UserModule.Domain.ConfirmationCodes.Exceptions;

namespace UniMeet.UserModule.Application.Users.ConfirmAccount;

public class ConfirmAccountCommandHandler(IConfirmationCodeRepository confirmationCodeRepository) 
    : ICommandHandler<ConfirmAccountCommand>
{
    public async Task HandleAsync(ConfirmAccountCommand request, CancellationToken cancellationToken = default)
    {
        var confirmationCode =
            await confirmationCodeRepository.GetByCodeAsync(request.ConfirmationCode, cancellationToken);

        if (confirmationCode == null)
            throw new CodeNotFoundException(request.ConfirmationCode);
        
        if (confirmationCode.IsExpired())
            throw new ConfirmationCodeExpiredException(request.ConfirmationCode);
        
        confirmationCode.User.Activate();
        confirmationCodeRepository.Delete(confirmationCode);
        await confirmationCodeRepository.SaveChangesAsync(cancellationToken);
    }
}