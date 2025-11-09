using MediatR;

namespace UniMeet.UniversityModule.Application.Universities.Commands.CreateUniversity;

public record CreateUniversityCommand(
    string Name,
    string Country,
    string Voivodeship,
    string City,
    string Address) : IRequest;