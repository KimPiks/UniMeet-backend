using UniMeet.Shared.Abstractions;

namespace UniMeet.UniversityModule.Application.Universities.Commands.DeleteUniversity;

public record DeleteUniversityCommand(int UniversityId) : IRequest;