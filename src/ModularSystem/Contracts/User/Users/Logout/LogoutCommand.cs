using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.User.Users.Logout;

public record LogoutCommand(string RefreshToken) : ICommand;