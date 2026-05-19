using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.User.Users.SearchUsers;

public record SearchUsersQuery(UserSearchProfile Profile, UserSearchFilters? Filters, int Offset, int Limit)
    : IQuery<IEnumerable<SearchUserDto>>;

