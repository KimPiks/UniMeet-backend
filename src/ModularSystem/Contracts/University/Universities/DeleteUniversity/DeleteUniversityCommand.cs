using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.University.Universities.DeleteUniversity;

public record DeleteUniversityCommand(int UniversityId) : ICommand;