using UniMeet.Shared.Abstractions;
using UniMeet.UserModule.Domain.UserDetails;

namespace UniMeet.UserModule.Application.Users.RegisterUser;

public record RegisterUserCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    Sex Sex) : ICommand;