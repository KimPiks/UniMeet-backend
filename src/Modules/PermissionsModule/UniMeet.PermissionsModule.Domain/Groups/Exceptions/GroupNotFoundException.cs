using UniMeet.Shared.Exceptions;

namespace PermissionsModule.Domain.Groups.Exceptions;

public class GroupNotFoundException(string name)
    : DomainException($"Group with name {name} not found.");

public class GroupWithIdNotFoundException(int id)
    : DomainException($"Group with id {id} not found.");