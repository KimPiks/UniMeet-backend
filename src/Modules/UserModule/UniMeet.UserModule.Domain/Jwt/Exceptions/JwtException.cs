using UniMeet.Shared.Exceptions;

namespace UniMeet.UserModule.Domain.Jwt.Exceptions;

public class JwtException(string message) : DomainException(message);