using UniMeet.Shared.Exceptions;

namespace PermissionsModule.Domain.Permissions.Exceptions;

public class PermissionAlreadyExistsException(string groupName, string name)
    : DomainException($"Permission with name {name} already exists in group {groupName}.");