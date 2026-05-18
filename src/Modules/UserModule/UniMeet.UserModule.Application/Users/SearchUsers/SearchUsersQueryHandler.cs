using UniMeet.Shared.Abstractions;
using UniMeet.UserEnrollmentModule.Domain.UserAffiliation;
using UniMeet.UserModule.Domain.Users;

namespace UniMeet.UserModule.Application.Users.SearchUsers;

public class SearchUsersQueryHandler(
    IUserRepository userRepository,
    IUserAffiliationRepository userAffiliationRepository)
    : IQueryHandler<SearchUsersQuery, IEnumerable<SearchUserDto>>
{
    private const int UniversityScore = 5;
    private const int FieldOfStudyScore = 4;
    private const int InterestScore = 2;
    private const int SexScore = 1;

    public async Task<IEnumerable<SearchUserDto>> HandleAsync(SearchUsersQuery request, CancellationToken cancellationToken = default)
    {
        var filters = request.Filters;
        IReadOnlyList<Guid>? fieldOfStudyUserIds = null;

        if (filters?.FieldOfStudyId is int fieldOfStudyId)
        {
            var ids = await userAffiliationRepository.GetUserIdsByFieldOfStudyIdAsync(fieldOfStudyId, cancellationToken);
            if (ids.Count == 0)
            {
                return Array.Empty<SearchUserDto>();
            }

            fieldOfStudyUserIds = ids;
        }

        var users = await userRepository.SearchAsync(
            filters?.UniversityId,
            filters?.Sex,
            filters?.InterestIds,
            fieldOfStudyUserIds,
            cancellationToken);

        if (users.Count == 0)
        {
            return Array.Empty<SearchUserDto>();
        }

        var userIds = users.Select(u => u.Id).ToList();
        var fieldOfStudyByUserId = await userAffiliationRepository
            .GetFieldOfStudyIdsByUserIdsAsync(userIds, cancellationToken);

        var profile = request.Profile;
        var profileInterestIds = new HashSet<int>(profile.InterestIds);

        var scoredUsers = users.Select(user =>
        {
            var hasFieldOfStudy = fieldOfStudyByUserId.TryGetValue(user.Id, out var candidateFieldOfStudyId);
            var fieldOfStudyValue = hasFieldOfStudy ? candidateFieldOfStudyId : (int?)null;
            var score = CalculateScore(user, fieldOfStudyValue, profile, profileInterestIds);

            return new
            {
                User = user,
                Score = score,
                FieldOfStudyId = fieldOfStudyValue
            };
        });

        var offset = Math.Max(0, request.Offset);
        var limit = request.Limit <= 0 ? 50 : request.Limit;

        return scoredUsers
            .OrderByDescending(x => x.Score)
            .ThenBy(x => x.User.LastName)
            .ThenBy(x => x.User.FirstName)
            .Skip(offset)
            .Take(limit)
            .Select(x => x.User.ToSearchDto(x.Score, x.FieldOfStudyId))
            .ToList();
    }

    private static int CalculateScore(User user, int? fieldOfStudyId, UserSearchProfile profile, HashSet<int> profileInterestIds)
    {
        var score = 0;

        if (user.UniversityId == profile.UniversityId)
        {
            score += UniversityScore;
        }

        if (fieldOfStudyId.HasValue && fieldOfStudyId.Value == profile.FieldOfStudyId)
        {
            score += FieldOfStudyScore;
        }

        if (user.UserDetail != null)
        {
            if (user.UserDetail.Sex == profile.Sex)
            {
                score += SexScore;
            }

            if (profileInterestIds.Count > 0 && user.UserDetail.Interests.Count > 0)
            {
                var matchedInterests = user.UserDetail.Interests.Count(i => profileInterestIds.Contains(i.Id));
                score += matchedInterests * InterestScore;
            }
        }

        return score;
    }
}
