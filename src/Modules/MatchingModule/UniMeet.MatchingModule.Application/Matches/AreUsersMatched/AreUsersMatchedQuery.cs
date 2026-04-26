using UniMeet.Shared.Abstractions;

namespace UniMeet.MatchingModule.Application.Matches.AreUsersMatched;

public record AreUsersMatchedQuery(Guid UserAId, Guid UserBId) : IQuery<bool>;
