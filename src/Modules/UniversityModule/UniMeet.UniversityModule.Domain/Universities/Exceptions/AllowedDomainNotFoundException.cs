using UniMeet.Shared.Exceptions;

namespace UniMeet.UniversityModule.Domain.Universities.Exceptions;

public sealed class AllowedDomainNotFoundException(int id) :
    DomainException($"Allowed email domain with id '{id}' was not found.");