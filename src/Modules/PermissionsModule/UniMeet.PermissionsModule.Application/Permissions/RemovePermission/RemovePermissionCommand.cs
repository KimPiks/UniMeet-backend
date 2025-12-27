using UniMeet.Shared.Abstractions;

namespace PermissionsModule.Application.Permissions.RemovePermission;

public record RemovePermissionCommand(int Id) : ICommand;