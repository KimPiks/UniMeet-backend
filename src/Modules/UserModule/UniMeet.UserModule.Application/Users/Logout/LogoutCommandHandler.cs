using UniMeet.Shared.Abstractions;
using UniMeet.UserModule.Domain.RefreshTokens;
using UniMeet.UserModule.Domain.RefreshTokens.Exceptions;

namespace UniMeet.UserModule.Application.Users.Logout;

public class LogoutCommandHandler(IRefreshTokenRepository refreshTokenRepository) : ICommandHandler<LogoutCommand>
{
    public async Task HandleAsync(LogoutCommand request, CancellationToken cancellationToken = default)
    {
        var token = await refreshTokenRepository.GetByTokenAsync(request.RefreshToken, cancellationToken);
        if (token == null)
            throw new TokenNotFoundException(request.RefreshToken);
        
        refreshTokenRepository.Delete(token);
        await refreshTokenRepository.SaveChangesAsync(cancellationToken);
    }
}