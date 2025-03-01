namespace Alexandria.Application.Abstractions.Repositories.Exceptions;

public sealed class AuthorAlreadyExistsException : Exception
{
    public AuthorAlreadyExistsException(string message)
        : base(message) { }

    public AuthorAlreadyExistsException(string message, Exception innerException)
        : base(message, innerException) { }

    public AuthorAlreadyExistsException() { }
}
