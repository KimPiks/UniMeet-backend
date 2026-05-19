using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.Matching.Matches.Unmatch;

public record UnmatchCommand(Guid RequestingUserId, Guid OtherUserId) : ICommand;
