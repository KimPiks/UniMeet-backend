using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.University.Universities.CreateUniversity;

public record CreateUniversityCommand(
    string Name,
    string Country,
    string Voivodeship,
    string City,
    string Address) : ICommand;