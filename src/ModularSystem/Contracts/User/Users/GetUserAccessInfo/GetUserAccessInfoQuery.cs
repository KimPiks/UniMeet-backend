using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.User.Users.GetUserAccessInfo;

public record GetUserAccessInfoQuery(Guid UserId) : IQuery<UserAccessInfoDto?>;
