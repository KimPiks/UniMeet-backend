using UniMeet.Shared.Exceptions;

namespace UniMeet.UniversityModule.Domain.Universities.Exceptions;

public sealed class InvalidCityNameException(string name) :
    DomainException($"The city name '{name}' is invalid.");