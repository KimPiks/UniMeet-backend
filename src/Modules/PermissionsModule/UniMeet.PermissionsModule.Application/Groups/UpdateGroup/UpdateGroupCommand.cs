using UniMeet.Shared.Abstractions;

namespace PermissionsModule.Application.Groups.UpdateGroup;

public record UpdateGroupCommand(int Id, string NewName) : ICommand;