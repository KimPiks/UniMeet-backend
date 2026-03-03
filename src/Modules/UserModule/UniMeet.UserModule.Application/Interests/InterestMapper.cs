using UniMeet.UserModule.Domain.Interests;

namespace UniMeet.UserModule.Application.Interests;

public static class InterestMapper
{
    public static InterestDto ToDto(this Interest interest)
    {
        return new InterestDto
        {
            Id = interest.Id,
            Name = interest.Name
        };
    }
}

