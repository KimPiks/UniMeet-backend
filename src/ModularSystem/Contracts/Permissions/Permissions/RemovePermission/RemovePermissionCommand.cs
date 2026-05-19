using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.Permissions.Permissions.RemovePermission;

public record RemovePermissionCommand(int Id) : ICommand;