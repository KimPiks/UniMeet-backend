using UniMeet.UserModule.Domain.UserDetails;

namespace UniMeet.UserModule.Application.Users.SearchUsers;

public record UserSearchFilters(
    int? UniversityId,
    int? FieldOfStudyId,
    IReadOnlyCollection<int>? InterestIds,
    Sex? Sex);

