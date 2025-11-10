using UniMeet.Shared.Exceptions;

namespace UniMeet.UniversityModule.Domain.Universities.Exceptions;

public sealed class FieldOfStudyAlreadyExistsException(string name) : 
    DomainException($"Field of study with name '{name}' already exists.");