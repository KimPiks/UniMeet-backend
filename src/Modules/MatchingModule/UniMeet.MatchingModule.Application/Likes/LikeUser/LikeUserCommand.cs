using UniMeet.Shared.Abstractions;

namespace UniMeet.MatchingModule.Application.Likes.LikeUser;

public record LikeUserCommand(Guid LikerId, Guid LikedId) : ICommand<LikeResultDto>;
