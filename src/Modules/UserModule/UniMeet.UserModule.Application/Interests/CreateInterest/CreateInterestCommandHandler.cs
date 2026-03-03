using UniMeet.Shared.Abstractions;
using UniMeet.UserModule.Domain.Interests;

namespace UniMeet.UserModule.Application.Interests.CreateInterest;

public class CreateInterestCommandHandler(IInterestRepository interestRepository) 
    : ICommandHandler<CreateInterestCommand, InterestDto>
{
    public async Task<InterestDto> HandleAsync(CreateInterestCommand request, CancellationToken cancellationToken = default)
    {
        var interest = new Interest { Name = request.Name };
        
        await interestRepository.AddAsync(interest, cancellationToken);
        await interestRepository.SaveChangesAsync(cancellationToken);
        
        return interest.ToDto();
    }
}

