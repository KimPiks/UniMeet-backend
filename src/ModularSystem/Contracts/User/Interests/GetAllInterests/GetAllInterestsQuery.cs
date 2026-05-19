using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.User.Interests.GetAllInterests;

public record GetAllInterestsQuery(int Offset, int Limit) : IQuery<IEnumerable<InterestDto>>;

