using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.User.Users.SetGroup;

public record SetGroupCommand(Guid UserId, int GroupId) : ICommand;