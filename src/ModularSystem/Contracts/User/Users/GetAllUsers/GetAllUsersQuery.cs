using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.User.Users.GetAllUsers;

public record GetAllUsersQuery(int Offset, int Limit) : IQuery<IEnumerable<UserDto>>;