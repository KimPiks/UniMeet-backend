using UniMeet.Shared.Abstractions;

namespace UniMeet.UserModule.Application.Interests.GetInterestById;

public record GetInterestByIdQuery(int InterestId) : IQuery<InterestDto?>;

