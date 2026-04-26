using UniMeet.Shared.Abstractions;

namespace UniMeet.MatchingModule.Application.Matches.Unmatch;

public record UnmatchCommand(Guid RequestingUserId, Guid OtherUserId) : ICommand;
