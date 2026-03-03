using UniMeet.Shared.Abstractions;
using UniMeet.UserModule.Domain.Interests;

namespace UniMeet.UserModule.Application.Interests.DeleteInterest;

public class DeleteInterestCommandHandler(IInterestRepository interestRepository) 
    : ICommandHandler<DeleteInterestCommand>
{
    public async Task HandleAsync(DeleteInterestCommand request, CancellationToken cancellationToken = default)
    {
        var interest = await interestRepository.GetByIdAsync(request.InterestId, cancellationToken);
        if (interest == null)
            throw new KeyNotFoundException($"Interest with id {request.InterestId} not found");
        
        interestRepository.Delete(interest);
        await interestRepository.SaveChangesAsync(cancellationToken);
    }
}

