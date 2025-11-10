using UniMeet.Shared.Exceptions;

namespace UniMeet.UniversityModule.Domain.Universities.Exceptions;

public sealed class InvalidAllowedEmailDomainNameException(string name) :
    DomainException($"The allowed email domain name '{name}' is invalid.");
