using UniMeet.Shared.Exceptions;

namespace UniMeet.UserModule.Domain.RefreshTokens.Exceptions;

public class TokenExpired(string token)
    : DomainException($"Token '{token}' expired.");