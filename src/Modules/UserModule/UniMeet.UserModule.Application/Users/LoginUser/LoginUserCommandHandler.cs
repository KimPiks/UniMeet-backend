using UniMeet.Shared.Abstractions;
using UniMeet.UserModule.Domain.Models;
using UniMeet.UserModule.Domain.RefreshTokens;
using UniMeet.UserModule.Domain.Services;
using UniMeet.UserModule.Domain.Users;
using UniMeet.UserModule.Domain.Users.Exceptions;

namespace UniMeet.UserModule.Application.Users.LoginUser;

public class LoginUserCommandHandler(IUserRepository userRepository,
    IRefreshTokenRepository refreshTokenRepository,
    IPasswordHasher passwordHasher,
    IJwtService jwtService)
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

        var accessToken = jwtService.GenerateToken(user.Id, TokenType.Access);
        var refreshToken = jwtService.GenerateToken(user.Id, TokenType.Refresh);
        
        var expiresAtUtc = jwtService.GetExpires(TokenType.Refresh);
        var authRefreshToken = new RefreshToken(refreshToken, expiresAtUtc, user.Id);
        await refreshTokenRepository.AddAsync(authRefreshToken, cancellationToken);
        await refreshTokenRepository.SaveChangesAsync(cancellationToken);

        return new LoginTokens(accessToken, refreshToken);
    }
}