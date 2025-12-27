using UniMeet.Shared.Abstractions;

namespace UniMeet.UserModule.Application.Users.SetGroup;

public record SetGroupCommand(Guid UserId, int GroupId) : ICommand;