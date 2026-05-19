using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.Permissions.Groups.UpdateGroup;

public record UpdateGroupCommand(int Id, string NewName) : ICommand;