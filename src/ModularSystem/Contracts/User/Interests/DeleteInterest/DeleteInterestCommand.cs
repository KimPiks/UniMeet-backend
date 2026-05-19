using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.User.Interests.DeleteInterest;

public record DeleteInterestCommand(int InterestId) : ICommand;

