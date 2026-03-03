using UniMeet.Shared.Abstractions;
using UniMeet.UserModule.Domain.UserDetails;
using UniMeet.UserModule.Domain.Interests;

namespace UniMeet.UserModule.Application.UserDetails.UpdateUserDetail;

public class UpdateUserDetailCommandHandler(IUserDetailRepository userDetailRepository, IInterestRepository interestRepository) 
    : ICommandHandler<UpdateUserDetailCommand, UserDetailDto>
{
    public async Task<UserDetailDto> HandleAsync(UpdateUserDetailCommand request, CancellationToken cancellationToken = default)
    {
        var userDetail = await userDetailRepository.GetByIdAsync(request.UserDetailId, cancellationToken);
        if (userDetail == null)
            throw new KeyNotFoundException($"UserDetail with id {request.UserDetailId} not found");
        
        if (request.InterestIds != null && request.InterestIds.Count > 0)
        {
            userDetail.Interests.Clear();
            
            foreach (var interestId in request.InterestIds)
            {
                var interest = await interestRepository.GetByIdAsync(interestId, cancellationToken);
                if (interest == null)
                    throw new KeyNotFoundException($"Interest with id {interestId} not found");
                
                userDetail.AddInterest(interest);
            }
        }
        
        await userDetailRepository.SaveChangesAsync(cancellationToken);
        
        return userDetail.ToDto();
    }
}

