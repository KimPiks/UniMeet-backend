using UniMeet.Shared.Abstractions;

namespace PermissionsModule.Application.Groups.RemoveGroup;

public record RemoveGroupCommand(int Id) : ICommand;