using UniMeet.Shared.Exceptions;

namespace UniMeet.UniversityModule.Domain.Universities.Exceptions;

public sealed class DepartmentNotFoundException(int id) :
    DomainException($"The department with id '{id}' was not found.");