using UniMeet.Shared.Exceptions;

namespace UniMeet.UserModule.Domain.Users.Exceptions;

public class EmailDomainNotAllowedException(string name) :
    DomainException($"The email domain '{name}' is not allowed.");