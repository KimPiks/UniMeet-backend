using ModularSystem.Contracts.User.UserDetails;

namespace ModularSystem.Contracts.User.Users.SearchUsers;

public record UserSearchFilters(
    int? UniversityId,
    int? FieldOfStudyId,
    IReadOnlyCollection<int>? InterestIds,
    Sex? Sex);

