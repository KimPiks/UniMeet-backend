using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.Matching.Matches.AreUsersMatched;

public record AreUsersMatchedQuery(Guid UserAId, Guid UserBId) : IQuery<bool>;
