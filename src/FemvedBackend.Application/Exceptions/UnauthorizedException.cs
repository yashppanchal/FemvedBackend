namespace FemvedBackend.Application.Exceptions;

/// <summary>
/// Represents an unauthorized access attempt.
/// </summary>
public sealed class UnauthorizedException : Exception
{
    public UnauthorizedException(string message)
        : base(message)
    {
    }
}
