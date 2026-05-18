using UniMeet.Shared.Exceptions;

namespace UniMeet.UserModule.Domain.Users.Exceptions;

public class DefaultGroupNotFoundException(string name)
    : DomainException($"Default group '{name}' was not found.");
