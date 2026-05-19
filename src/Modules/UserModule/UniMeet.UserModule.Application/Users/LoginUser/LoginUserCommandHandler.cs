using ModularSystem.Contracts.Permissions.Permissions.GetPermissionsForGroup;
using UniMeet.Shared.Abstractions;
using UniMeet.UserModule.Domain.Models;
using UniMeet.UserModule.Domain.RefreshTokens;
using UniMeet.UserModule.Domain.Services;
using UniMeet.UserModule.Domain.Users;
using UniMeet.UserModule.Domain.Users.Exceptions;
using LoginTokens = ModularSystem.Contracts.User.Models.LoginTokens;

namespace UniMeet.UserModule.Application.Users.LoginUser;

public class LoginUserCommandHandler(
    IUserRepository userRepository,
    IRefreshTokenRepository refreshTokenRepository,
    IPasswordHasher passwordHasher,
    IJwtService jwtService,
    IMediator mediator)
    : ICommandHandler<LoginUserCommand, LoginTokens>
{
    public async Task<LoginTokens> HandleAsync(LoginUserCommand request, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (user == null)
            throw new UserNotFoundException(request.Email);

        if (!user.IsActive)
            throw new UserNotActiveException(request.Email);

        if (!passwordHasher.Verify(request.Password, user.PasswordHash))
            throw new InvalidPasswordException(request.Email);

        var permissions = await mediator.SendAsync(new GetPermissionsForGroupQuery(user.GroupId), cancellationToken);
        var accessToken = jwtService.GenerateAccessToken(
            user.Id,
            user.IsActive,
            user.GroupId,
            permissions.Select(permission => permission.PermissionName));
        var refreshToken = jwtService.GenerateRefreshToken(user.Id);

        var expiresAtUtc = jwtService.GetExpires(TokenType.Refresh);
        var authRefreshToken = new RefreshToken(refreshToken, expiresAtUtc, user.Id);
        await refreshTokenRepository.AddAsync(authRefreshToken, cancellationToken);
        await refreshTokenRepository.SaveChangesAsync(cancellationToken);

        return new LoginTokens(accessToken, refreshToken);
    }
}
