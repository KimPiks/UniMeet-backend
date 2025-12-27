using UniMeet.Shared.Abstractions;

namespace PermissionsModule.Application.Permissions.AddPermission;

public record AddPermissionCommand(int GroupId, string PermissionName) : ICommand;