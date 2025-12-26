using UniMeet.Shared.Exceptions;

namespace UniMeet.UserModule.Domain.Users.Exceptions;

public class InvalidPasswordException(string email)
    : DomainException($"Invalid password for user with email '{email}'.");