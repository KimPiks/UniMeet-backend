namespace UniMeet.Shared.Exceptions;

public class ValidationException : Exception
{
    public IReadOnlyList<string> Errors { get; }

    public ValidationException(IEnumerable<string> errors)
        : base("Validation failed")
    {
        Errors = new List<string>(errors);
    }

    public ValidationException(string error)
        : this(new[] { error })
    {
    }

    public override string ToString()
    {
        return $"{Message}: {string.Join("; ", Errors)}";
    }
}