using UniMeet.Shared.Exceptions;

namespace UniMeet.UniversityModule.Domain.Universities.Exceptions;

public sealed class InvalidUniversityNameException(string name)
    : DomainException($"University name `{name}` is invalid.");