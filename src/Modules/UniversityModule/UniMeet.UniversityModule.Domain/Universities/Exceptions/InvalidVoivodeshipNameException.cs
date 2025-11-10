using UniMeet.Shared.Exceptions;

namespace UniMeet.UniversityModule.Domain.Universities.Exceptions;

public sealed class InvalidVoivodeshipNameException(string name) :
    DomainException($"The voivodeship name '{name}' is invalid.");