using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.Permissions.Groups.RemoveGroup;

public record RemoveGroupCommand(int Id) : ICommand;