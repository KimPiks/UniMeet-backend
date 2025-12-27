using UniMeet.Shared.Abstractions;

namespace UniMeet.UserModule.Application.Users.Logout;

public record LogoutCommand(string RefreshToken) : ICommand;