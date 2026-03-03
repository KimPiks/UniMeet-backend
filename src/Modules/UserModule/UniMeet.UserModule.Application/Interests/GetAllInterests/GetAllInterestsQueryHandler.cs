using UniMeet.Shared.Abstractions;
using UniMeet.UserModule.Domain.Interests;

namespace UniMeet.UserModule.Application.Interests.GetAllInterests;

public class GetAllInterestsQueryHandler(IInterestRepository interestRepository) 
    : IQueryHandler<GetAllInterestsQuery, IEnumerable<InterestDto>>
{
    public async Task<IEnumerable<InterestDto>> HandleAsync(GetAllInterestsQuery request, CancellationToken cancellationToken = default)
    {
        var interests = await interestRepository.GetAllAsync(request.Offset, request.Limit, cancellationToken);
        return interests.Select(interest => interest.ToDto()).ToList();
    }
}

