using UniMeet.Shared.Abstractions;
using ModularSystem.Contracts.User.UserDetails;

namespace ModularSystem.Contracts.User.Users.RegisterUser;

public record RegisterUserCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    Sex Sex) : ICommand;