using UniMeet.Shared.Abstractions;
using UniMeet.UserModule.Domain.Models;
using UniMeet.UserModule.Domain.RefreshTokens;
using UniMeet.UserModule.Domain.RefreshTokens.Exceptions;
using UniMeet.UserModule.Domain.Services;

namespace UniMeet.UserModule.Application.RefreshTokens.RefreshTokens;

public class RefreshTokensCommandHandler(IRefreshTokenRepository refreshTokenRepository,
    IJwtService jwtService) 
    : ICommandHandler<RefreshTokensCommand, LoginTokens>
{
    public async Task<LoginTokens> HandleAsync(RefreshTokensCommand request, CancellationToken cancellationToken = default)
    {
        var refreshToken = await refreshTokenRepository.GetByTokenAsync(request.RefreshToken, cancellationToken);
        if (refreshToken == null)
            throw new TokenNotFoundException(request.RefreshToken);

        if (refreshToken.IsExpired())
            throw new TokenExpired(request.RefreshToken);

        refreshTokenRepository.Delete(refreshToken);
        
        var newAccessToken = jwtService.GenerateToken(refreshToken.UserId, TokenType.Access);
        var newRefreshToken = jwtService.GenerateToken(refreshToken.UserId, TokenType.Refresh);
        
        var authRefreshToken = new RefreshToken(newRefreshToken, jwtService.GetExpires(TokenType.Refresh), refreshToken.UserId);
        await refreshTokenRepository.AddAsync(authRefreshToken, cancellationToken);
        await refreshTokenRepository.SaveChangesAsync(cancellationToken);
        
        return new LoginTokens(newAccessToken, newRefreshToken);
    }
}