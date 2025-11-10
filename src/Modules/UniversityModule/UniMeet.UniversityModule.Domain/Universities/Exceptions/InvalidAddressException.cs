using UniMeet.Shared.Exceptions;

namespace UniMeet.UniversityModule.Domain.Universities.Exceptions;

public sealed class InvalidAddressException(string name) :
    DomainException($"The address '{name}' is invalid.");
