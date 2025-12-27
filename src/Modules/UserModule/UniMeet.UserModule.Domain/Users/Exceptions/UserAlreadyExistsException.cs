using UniMeet.Shared.Exceptions;

namespace UniMeet.UserModule.Domain.Users.Exceptions;

public class UserAlreadyExistsException(string email)
    : DomainException($"User with email '{email}' already exists.");