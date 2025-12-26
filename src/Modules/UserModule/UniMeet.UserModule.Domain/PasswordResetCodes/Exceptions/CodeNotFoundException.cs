using UniMeet.Shared.Exceptions;

namespace UniMeet.UserModule.Domain.PasswordResetCodes.Exceptions;

public class CodeNotFoundException(Guid code) 
    : DomainException($"Password reset code '{code}' not found.");