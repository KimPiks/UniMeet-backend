using UniMeet.Shared.Abstractions;

namespace UniMeet.UserModule.Application.Users.GetAllUsers;

public record GetAllUsersQuery(int Offset, int Limit) : IQuery<IEnumerable<UserDto>>;