using UniMeet.Shared.Abstractions;
using UniMeet.UserModule.Domain.Models;
using UniMeet.UserModule.Domain.RefreshTokens;
using UniMeet.UserModule.Domain.RefreshTokens.Exceptions;
using UniMeet.UserModule.Domain.Services;

namespace UniMeet.UserModule.Application.Users.Logout;

public class LogoutCommandHandler(
    IRefreshTokenRepository refreshTokenRepository,
    IJwtService jwtService) : ICommandHandler<LogoutCommand>
{
    public async Task HandleAsync(LogoutCommand request, CancellationToken cancellationToken = default)
    {
        jwtService.GetUserIdFromToken(request.RefreshToken, TokenType.Refresh);

        var token = await refreshTokenRepository.GetByTokenAsync(request.RefreshToken, cancellationToken);
        if (token == null)
            throw new TokenNotFoundException(request.RefreshToken);

        refreshTokenRepository.Delete(token);
        await refreshTokenRepository.SaveChangesAsync(cancellationToken);
    }
}
