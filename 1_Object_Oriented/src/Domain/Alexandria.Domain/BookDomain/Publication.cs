namespace Alexandria.Domain.BookDomain;

public class Publication
{
    public Publication(long id, DateTimeOffset publicationDate, ICollection<long> authorsIds)
    {
        Id = id;
        PublicationDate = publicationDate;
        AuthorsIds = authorsIds;
    }

    public long Id { get; }
    public DateTimeOffset PublicationDate { get; }
    public ICollection<long> AuthorsIds { get; }

    public static Publication CreateTransient(
        DateTimeOffset publicationDate,
        ICollection<long> authorsIds
    )
    {
        ArgumentNullException.ThrowIfNull(authorsIds, nameof(authorsIds));
        return authorsIds.Count == 0
            ? throw new ArgumentException($"'{nameof(authorsIds)}' cannot be empty.")
            : new Publication(0, publicationDate, authorsIds);
    }
}
