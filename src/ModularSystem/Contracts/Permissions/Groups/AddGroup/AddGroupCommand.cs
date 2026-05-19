using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.Permissions.Groups.AddGroup;

public record AddGroupCommand(string Name) : ICommand;