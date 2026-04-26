namespace UniMeet.MatchingModule.Application.Likes;

public record LikeResultDto(bool Matched, Guid? MatchId, bool JustMatched);
