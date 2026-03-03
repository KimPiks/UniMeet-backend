using UniMeet.Shared.Abstractions;

namespace UniMeet.UserModule.Application.Interests.CreateInterest;

public record CreateInterestCommand(string Name) : ICommand<InterestDto>;

