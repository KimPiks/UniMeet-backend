using UniMeet.UserModule.Domain.Models;

namespace UniMeet.UserModule.Domain.Services;

public interface IJwtService
{
    string GenerateToken(Guid userId, TokenType tokenType);
    Guid GetUserIdFromToken(string token);
    DateTime GetExpires(TokenType tokenType);
}