using UniMeet.Shared.Exceptions;

namespace PermissionsModule.Domain.Groups.Exceptions;

public class GroupAlreadyExistsException(string name) 
    : DomainException($"Group with name {name} already exists.");