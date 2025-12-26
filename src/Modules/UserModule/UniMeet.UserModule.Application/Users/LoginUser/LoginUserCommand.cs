using UniMeet.Shared.Abstractions;
using UniMeet.UserModule.Domain.Models;

namespace UniMeet.UserModule.Application.Users.LoginUser;

public record LoginUserCommand(string Email, string Password) : ICommand<LoginTokens>;