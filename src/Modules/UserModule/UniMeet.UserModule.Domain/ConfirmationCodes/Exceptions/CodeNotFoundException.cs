using UniMeet.Shared.Exceptions;

namespace UniMeet.UserModule.Domain.ConfirmationCodes.Exceptions;

public class CodeNotFoundException(Guid code)
    : DomainException($"Confirmation code '{code}' not found.");