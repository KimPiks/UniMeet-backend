using UniMeet.Shared.Abstractions;
using UniMeet.UserModule.Domain.Interests;

namespace UniMeet.UserModule.Application.Interests.GetInterestById;

public class GetInterestByIdQueryHandler(IInterestRepository interestRepository) 
    : IQueryHandler<GetInterestByIdQuery, InterestDto?>
{
    public async Task<InterestDto?> HandleAsync(GetInterestByIdQuery request, CancellationToken cancellationToken = default)
    {
        var interest = await interestRepository.GetByIdAsync(request.InterestId, cancellationToken);
        return interest?.ToDto();
    }
}

