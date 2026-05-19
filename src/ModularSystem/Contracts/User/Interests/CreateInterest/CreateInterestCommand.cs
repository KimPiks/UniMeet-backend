using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.User.Interests.CreateInterest;

public record CreateInterestCommand(string Name) : ICommand<InterestDto>;

