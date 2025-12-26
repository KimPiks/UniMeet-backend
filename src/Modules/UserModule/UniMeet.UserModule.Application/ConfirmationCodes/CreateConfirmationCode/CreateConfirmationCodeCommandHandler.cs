using UniMeet.Shared.Abstractions;
using UniMeet.UserModule.Domain.ConfirmationCodes;

namespace UniMeet.UserModule.Application.ConfirmationCodes.CreateConfirmationCode;

public class CreateConfirmationCodeCommandHandler(IConfirmationCodeRepository confirmationCodeRepository) : ICommandHandler<CreateConfirmationCodeCommand, Guid>
{
    public async Task<Guid> HandleAsync(CreateConfirmationCodeCommand request, CancellationToken cancellationToken = default)
    {
        var expirationTime = DateTime.UtcNow.AddHours(1);
        var confirmationCode = new ConfirmationCode(request.UserId, expirationTime);
        
        await confirmationCodeRepository.AddAsync(confirmationCode, cancellationToken);
        await confirmationCodeRepository.SaveChangesAsync(cancellationToken);

        return confirmationCode.Code;
    }
}