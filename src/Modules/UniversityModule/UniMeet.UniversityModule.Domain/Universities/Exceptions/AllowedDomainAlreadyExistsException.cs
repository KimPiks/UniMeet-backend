using UniMeet.Shared.Exceptions;

namespace UniMeet.UniversityModule.Domain.Universities.Exceptions;

public sealed class AllowedDomainAlreadyExistsException(string name) : 
    DomainException($"The allowed email domain '{name}' already exists.");