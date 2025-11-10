namespace UniMeet.Shared.Abstractions;

public interface IRequest<TResult> { }
public interface IRequest : IRequest<Unit> { }

/// <summary>
/// Represents a void result for requests that do not return a value.
/// </summary>
public readonly struct Unit
{
    public static readonly Unit Value = new Unit();
}