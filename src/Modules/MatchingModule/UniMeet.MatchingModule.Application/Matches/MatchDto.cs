namespace UniMeet.MatchingModule.Application.Matches;

public record MatchDto(
    Guid Id,
    Guid User1Id,
    Guid User2Id,
    DateTime CreatedAt);
