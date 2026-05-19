namespace ModularSystem.Contracts.User.Users.GetUserAccessInfo;

public record UserAccessInfoDto(Guid Id, bool IsActive, int GroupId);
