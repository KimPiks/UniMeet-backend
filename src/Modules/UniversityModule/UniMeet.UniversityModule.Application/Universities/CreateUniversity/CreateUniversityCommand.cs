using UniMeet.Shared.Abstractions;

namespace UniMeet.UniversityModule.Application.Universities.CreateUniversity;

public record CreateUniversityCommand(
    string Name,
    string Country,
    string Voivodeship,
    string City,
    string Address) : ICommand;