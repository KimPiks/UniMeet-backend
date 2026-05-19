using ModularSystem.Contracts.Permissions.Permissions.GetPermissionsForGroup;
using UniMeet.Shared.Abstractions;
using UniMeet.UserModule.Domain.Models;
using UniMeet.UserModule.Domain.RefreshTokens;
using UniMeet.UserModule.Domain.RefreshTokens.Exceptions;
using UniMeet.UserModule.Domain.Services;
using UniMeet.UserModule.Domain.Users;
using UniMeet.UserModule.Domain.Users.Exceptions;
using LoginTokens = ModularSystem.Contracts.User.Models.LoginTokens;

namespace UniMeet.UserModule.Application.RefreshTokens.RefreshTokens;

public class RefreshTokensCommandHandler(
    IRefreshTokenRepository refreshTokenRepository,
    IUserRepository userRepository,
    IJwtService jwtService,
    IMediator mediator)
    : ICommandHandler<RefreshTokensCommand, LoginTokens>
{
    public async Task<LoginTokens> HandleAsync(RefreshTokensCommand request, CancellationToken cancellationToken = default)
    {
        var tokenUserId = jwtService.GetUserIdFromToken(request.RefreshToken, TokenType.Refresh);
        var refreshToken = await refreshTokenRepository.GetByTokenAsync(request.RefreshToken, cancellationToken);
        if (refreshToken == null)
            throw new TokenNotFoundException(request.RefreshToken);

        if (refreshToken.UserId != tokenUserId)
            throw new TokenNotFoundException(request.RefreshToken);

        if (refreshToken.IsExpired())
            throw new TokenExpired(request.RefreshToken);

        var user = await userRepository.GetByIdAsync(refreshToken.UserId, cancellationToken)
                   ?? throw new UserNotFoundException(refreshToken.UserId.ToString());

        if (!user.IsActive)
            throw new UserNotActiveException(user.Email);

        refreshTokenRepository.Delete(refreshToken);

        var permissions = await mediator.SendAsync(new GetPermissionsForGroupQuery(user.GroupId), cancellationToken);
        var newAccessToken = jwtService.GenerateAccessToken(
            user.Id,
            user.IsActive,
            user.GroupId,
            permissions.Select(permission => permission.PermissionName));
        var newRefreshToken = jwtService.GenerateRefreshToken(user.Id);

        var authRefreshToken = new RefreshToken(newRefreshToken, jwtService.GetExpires(TokenType.Refresh), refreshToken.UserId);
        await refreshTokenRepository.AddAsync(authRefreshToken, cancellationToken);
        await refreshTokenRepository.SaveChangesAsync(cancellationToken);

        return new LoginTokens(newAccessToken, newRefreshToken);
    }
}
