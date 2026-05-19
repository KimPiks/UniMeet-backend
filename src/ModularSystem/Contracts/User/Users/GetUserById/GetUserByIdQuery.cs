using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.User.Users.GetUserById;

public record GetUserByIdQuery(Guid Id) : IQuery<UserDto?>;

