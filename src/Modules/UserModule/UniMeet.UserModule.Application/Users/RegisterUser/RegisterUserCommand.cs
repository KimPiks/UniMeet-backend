using UniMeet.Shared.Abstractions;

namespace UniMeet.UserModule.Application.Users.RegisterUser;

public record RegisterUserCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password) : ICommand;