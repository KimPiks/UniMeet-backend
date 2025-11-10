using UniMeet.Shared.Exceptions;

namespace UniMeet.UniversityModule.Domain.Universities.Exceptions;

public sealed class UniversityNotFoundException(int id) :
    DomainException($"The university with id '{id}' was not found.");