using UniMeet.Shared.Exceptions;

namespace UniMeet.UserModule.Domain.ConfirmationCodes.Exceptions;

public class ConfirmationCodeExpiredException(Guid code)
    : DomainException($"Confirmation code '{code}' expired.");