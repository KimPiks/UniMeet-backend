using UniMeet.Shared.Abstractions;
using UniMeet.UserModule.Domain.Models;

namespace UniMeet.UserModule.Application.RefreshTokens.RefreshTokens;

public record RefreshTokensCommand(string RefreshToken) : ICommand<LoginTokens>;