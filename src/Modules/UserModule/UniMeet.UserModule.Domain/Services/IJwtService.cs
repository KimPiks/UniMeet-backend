using UniMeet.UserModule.Domain.Models;

namespace UniMeet.UserModule.Domain.Services;

public interface IJwtService
{
    string GenerateAccessToken(Guid userId, bool isActive, int groupId, IEnumerable<string> permissions);
    string GenerateRefreshToken(Guid userId);
    Guid GetUserIdFromToken(string token, TokenType expectedTokenType);
    DateTime GetExpires(TokenType tokenType);
}
