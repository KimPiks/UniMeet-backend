using UniMeet.Shared.Exceptions;

namespace UniMeet.UniversityModule.Domain.Universities.Exceptions;

public sealed class InvalidDepartmentNameException(string name) :
    DomainException($"Department name `{name}` is invalid.");
