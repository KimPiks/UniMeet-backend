using UniMeet.Shared.Exceptions;

namespace UniMeet.UserModule.Domain.Users.Exceptions;

public class UserNotFoundException(string email)
    : DomainException($"User with email '{email}' not found.");
    
public class UserWithIdNotFoundException(Guid id)
    : DomainException($"User with id '{id}' not found.");