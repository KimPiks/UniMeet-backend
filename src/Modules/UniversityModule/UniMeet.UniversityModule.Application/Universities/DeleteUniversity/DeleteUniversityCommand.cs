using UniMeet.Shared.Abstractions;

namespace UniMeet.UniversityModule.Application.Universities.DeleteUniversity;

public record DeleteUniversityCommand(int UniversityId) : ICommand;