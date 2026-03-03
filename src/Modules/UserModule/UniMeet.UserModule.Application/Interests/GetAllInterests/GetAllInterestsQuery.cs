using UniMeet.Shared.Abstractions;

namespace UniMeet.UserModule.Application.Interests.GetAllInterests;

public record GetAllInterestsQuery(int Offset, int Limit) : IQuery<IEnumerable<InterestDto>>;

