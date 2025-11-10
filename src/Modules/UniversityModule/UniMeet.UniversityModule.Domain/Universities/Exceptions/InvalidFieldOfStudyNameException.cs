using UniMeet.Shared.Exceptions;

namespace UniMeet.UniversityModule.Domain.Universities.Exceptions;

public sealed class InvalidFieldOfStudyNameException(string name) :
    DomainException($"Field of study name `{name}` is invalid.");