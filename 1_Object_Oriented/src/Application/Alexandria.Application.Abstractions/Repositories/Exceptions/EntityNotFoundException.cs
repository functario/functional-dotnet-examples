namespace Alexandria.Application.Abstractions.Repositories.Exceptions;

public sealed class EntityNotFoundException : Exception
{
    public static string DefaultMessage(long id) => $"Entity with Id '{id}' was not found.";

    public long Id { get; }

    public EntityNotFoundException(long id)
        : base(DefaultMessage(id))
    {
        Id = id;
    }

    public EntityNotFoundException(string message)
        : base(message) { }

    public EntityNotFoundException(string message, Exception innerException)
        : base(message, innerException) { }

    public EntityNotFoundException() { }
}
