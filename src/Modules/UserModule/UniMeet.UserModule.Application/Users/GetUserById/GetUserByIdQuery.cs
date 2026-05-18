using UniMeet.Shared.Abstractions;

namespace UniMeet.UserModule.Application.Users.GetUserById;

public record GetUserByIdQuery(Guid Id) : IQuery<UserDto?>;

