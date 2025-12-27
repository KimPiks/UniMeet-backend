using UniMeet.Shared.Exceptions;

namespace UniMeet.UserModule.Domain.RefreshTokens.Exceptions;

public class TokenNotFoundException(string token)
    : DomainException($"Token '{token}' not found.");