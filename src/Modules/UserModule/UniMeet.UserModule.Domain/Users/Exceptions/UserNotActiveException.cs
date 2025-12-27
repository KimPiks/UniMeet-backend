using UniMeet.Shared.Exceptions;

namespace UniMeet.UserModule.Domain.Users.Exceptions;

public class UserNotActiveException(string email)
    : DomainException($"User with email '{email}' is not active.");