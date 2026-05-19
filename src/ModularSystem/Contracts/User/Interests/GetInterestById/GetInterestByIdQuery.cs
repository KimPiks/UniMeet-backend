using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.User.Interests.GetInterestById;

public record GetInterestByIdQuery(int InterestId) : IQuery<InterestDto?>;

