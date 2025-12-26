using UniMeet.Shared.Abstractions;

namespace PermissionsModule.Application.Groups.AddGroup;

public record AddGroupCommand(string Name) : ICommand;