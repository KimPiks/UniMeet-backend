using UniMeet.Shared.Exceptions;

namespace UniMeet.UserModule.Domain.PasswordResetCodes.Exceptions;

public class CodeExpiredException(Guid code) 
    : DomainException($"Password reset code '{code}' expired.");