using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.University.Universities.UpdateUniversity;

public record UpdateUniversityCommand(
    int UniversityId,
    string Name,
    string Country,
    string Voivodeship,
    string City,
    string Address) : ICommand;