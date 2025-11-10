using UniMeet.Shared.Exceptions;

namespace UniMeet.UniversityModule.Domain.Universities.Exceptions;

public sealed class DepartmentAlreadyExistsException(string name) : 
    DomainException($"Department with name '{name}' already exists.");