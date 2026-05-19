using UniMeet.Shared.Abstractions;
using ModularSystem.Contracts.User.Models;

namespace ModularSystem.Contracts.User.RefreshTokens.RefreshTokens;

public record RefreshTokensCommand(string RefreshToken) : ICommand<LoginTokens>;