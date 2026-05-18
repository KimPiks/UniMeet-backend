using UniMeet.Shared.Abstractions;

namespace UniMeet.UserModule.Application.Users.SearchUsers;

public record SearchUsersQuery(UserSearchProfile Profile, UserSearchFilters? Filters, int Offset, int Limit)
    : IQuery<IEnumerable<SearchUserDto>>;

