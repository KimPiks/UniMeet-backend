using UniMeet.MatchingModule.Domain.Likes;
using UniMeet.MatchingModule.Domain.Matches;
using UniMeet.Shared.Abstractions;

namespace UniMeet.MatchingModule.Application.Matches.Unmatch;

public class UnmatchCommandHandler(
    IMatchRepository matchRepository,
    ILikeRepository likeRepository)
    : ICommandHandler<UnmatchCommand>
{
    public async Task HandleAsync(UnmatchCommand request, CancellationToken cancellationToken = default)
    {
        var match = await matchRepository.GetByUsersAsync(request.RequestingUserId, request.OtherUserId, cancellationToken);
        if (match is null)
            return;

        matchRepository.Delete(match);

        var likeAtoB = await likeRepository.GetAsync(request.RequestingUserId, request.OtherUserId, cancellationToken);
        if (likeAtoB is not null)
            likeRepository.Delete(likeAtoB);

        var likeBtoA = await likeRepository.GetAsync(request.OtherUserId, request.RequestingUserId, cancellationToken);
        if (likeBtoA is not null)
            likeRepository.Delete(likeBtoA);

        await matchRepository.SaveChangesAsync(cancellationToken);
        await likeRepository.SaveChangesAsync(cancellationToken);
    }
}
