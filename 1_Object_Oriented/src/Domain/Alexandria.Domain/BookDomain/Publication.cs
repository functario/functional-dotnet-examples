namespace Alexandria.Domain.BookDomain;

public class Publication
{
    public Publication(
        long id,
        long bookId,
        DateTimeOffset publicationDate,
        ICollection<long> authorsIds
    )
    {
        Id = id;
        BookId = bookId;
        PublicationDate = publicationDate;
        AuthorsIds = authorsIds;
    }

    public long Id { get; }
    public long BookId { get; private set; }
    public DateTimeOffset PublicationDate { get; }
    public ICollection<long> AuthorsIds { get; }
    private bool IsTransient { get; set; }

    public static Publication CreateTransient(
        DateTimeOffset publicationDate,
        ICollection<long> authorsIds
    )
    {
        ArgumentNullException.ThrowIfNull(authorsIds, nameof(authorsIds));
        return authorsIds.Count == 0
            ? throw new ArgumentException($"'{nameof(authorsIds)}' cannot be empty.")
            : new Publication(0, 0, publicationDate, authorsIds) { IsTransient = true };
    }

    // This is bad! It is a glorify setter.
    public Publication AssociateBookId(long bookId)
    {
        if (IsTransient)
        {
            BookId = bookId;
            IsTransient = false;
            return this;
        }

        throw new InvalidOperationException(
            $"Cannot replace BookId on a non-transient {nameof(Publication)}."
        );
    }
}
