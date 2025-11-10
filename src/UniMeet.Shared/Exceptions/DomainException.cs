namespace UniMeet.Shared.Exceptions;

public abstract class DomainException(string message) : Exception(message);