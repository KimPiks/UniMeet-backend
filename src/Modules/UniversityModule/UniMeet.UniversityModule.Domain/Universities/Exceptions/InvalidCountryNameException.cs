using UniMeet.Shared.Exceptions;

namespace UniMeet.UniversityModule.Domain.Universities.Exceptions;

public sealed class InvalidCountryNameException(string name) :
    DomainException($"The country name '{name}' is invalid.");