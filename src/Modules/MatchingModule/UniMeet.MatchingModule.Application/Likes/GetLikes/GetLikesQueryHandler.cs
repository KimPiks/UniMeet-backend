using UniMeet.MatchingModule.Domain.Likes;
using UniMeet.Shared.Abstractions;

namespace UniMeet.MatchingModule.Application.Likes.GetLikes;

public class GetLikesQueryHandler(ILikeRepository likeRepository)
    : IQueryHandler<GetLikesQuery, IEnumerable<Guid>>
{
    public async Task<IEnumerable<Guid>> HandleAsync(GetLikesQuery request, CancellationToken cancellationToken = default)
    {
        var likes = await likeRepository.GetByLikerIdAsync(request.UserId, cancellationToken);
        return likes.Select(l => l.LikedId);
    }
}

