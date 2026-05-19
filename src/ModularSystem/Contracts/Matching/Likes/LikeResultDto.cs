namespace ModularSystem.Contracts.Matching.Likes;

public record LikeResultDto(bool Matched, Guid? MatchId, bool JustMatched);
