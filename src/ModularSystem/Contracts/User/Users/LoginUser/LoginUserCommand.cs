using UniMeet.Shared.Abstractions;
using ModularSystem.Contracts.User.Models;

namespace ModularSystem.Contracts.User.Users.LoginUser;

public record LoginUserCommand(string Email, string Password) : ICommand<LoginTokens>;