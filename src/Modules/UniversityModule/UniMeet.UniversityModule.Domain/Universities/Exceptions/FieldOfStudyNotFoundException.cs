using UniMeet.Shared.Exceptions;

namespace UniMeet.UniversityModule.Domain.Universities.Exceptions;

public sealed class FieldOfStudyNotFoundException(int id) :
    DomainException($"Field of study with id '{id}' was not found.");