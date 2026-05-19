using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.Matching.Likes.LikeUser;

public record LikeUserCommand(Guid LikerId, Guid LikedId) : ICommand<LikeResultDto>;
