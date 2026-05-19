using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.Permissions.Permissions.AddPermission;

public record AddPermissionCommand(int GroupId, string PermissionName) : ICommand;