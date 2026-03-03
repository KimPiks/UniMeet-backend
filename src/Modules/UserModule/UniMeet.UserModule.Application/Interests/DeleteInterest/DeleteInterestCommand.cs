using UniMeet.Shared.Abstractions;

namespace UniMeet.UserModule.Application.Interests.DeleteInterest;

public record DeleteInterestCommand(int InterestId) : ICommand;

