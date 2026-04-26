using UniMeet.MatchingModule.Domain.Likes;
using UniMeet.MatchingModule.Domain.Matches;
using UniMeet.Shared.Abstractions;

namespace UniMeet.MatchingModule.Application.Likes.LikeUser;

public class LikeUserCommandHandler(
    ILikeRepository likeRepository,
    IMatchRepository matchRepository)
    : ICommandHandler<LikeUserCommand, LikeResultDto>
{
    public async Task<LikeResultDto> HandleAsync(LikeUserCommand request, CancellationToken cancellationToken = default)
    {
        if (request.LikerId == request.LikedId)
            throw new InvalidOperationException("A user cannot like themselves.");

        var existingMatch = await matchRepository.GetByUsersAsync(request.LikerId, request.LikedId, cancellationToken);
        if (existingMatch is not null)
            return new LikeResultDto(true, existingMatch.Id, false);

        var alreadyLiked = await likeRepository.ExistsAsync(request.LikerId, request.LikedId, cancellationToken);
        if (!alreadyLiked)
        {
            var like = Like.Create(request.LikerId, request.LikedId);
            await likeRepository.AddAsync(like, cancellationToken);
            await likeRepository.SaveChangesAsync(cancellationToken);
        }

        var reverseLike = await likeRepository.ExistsAsync(request.LikedId, request.LikerId, cancellationToken);
        if (!reverseLike)
            return new LikeResultDto(false, null, false);

        var match = Match.Create(request.LikerId, request.LikedId);
        await matchRepository.AddAsync(match, cancellationToken);
        await matchRepository.SaveChangesAsync(cancellationToken);

        return new LikeResultDto(true, match.Id, true);
    }
}
