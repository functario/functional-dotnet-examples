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
    public long BookId { get; }
    public DateTimeOffset PublicationDate { get; }
    public ICollection<long> AuthorsIds { get; }
}
