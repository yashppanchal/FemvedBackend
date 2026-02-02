using System.Collections.ObjectModel;

namespace FemvedBackend.Application.Exceptions;

public sealed class ValidationException : Exception
{
    public IReadOnlyDictionary<string, string[]> Errors { get; }

    public ValidationException(string field, string message)
        : this("Validation failed", new Dictionary<string, string[]> { [field] = new[] { message } })
    {
    }

    public ValidationException(string message, IDictionary<string, string[]> errors)
        : base(message)
    {
        Errors = new ReadOnlyDictionary<string, string[]>(errors);
    }
}
