using ModularSystem.Contracts.User.UserDetails;

namespace ModularSystem.Contracts.User.Users.SearchUsers;

public record UserSearchProfile(
    int UniversityId,
    int FieldOfStudyId,
    IReadOnlyCollection<int> InterestIds,
    Sex Sex);

