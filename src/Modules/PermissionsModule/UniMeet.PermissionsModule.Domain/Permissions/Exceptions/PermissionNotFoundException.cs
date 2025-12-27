using UniMeet.Shared.Exceptions;

namespace PermissionsModule.Domain.Permissions.Exceptions;

public class PermissionNotFoundException(int id)
    : DomainException($"Permission with id {id} not found.");