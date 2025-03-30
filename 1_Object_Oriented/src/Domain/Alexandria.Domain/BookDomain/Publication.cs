namespace Alexandria.Domain.BookDomain;

public class Publication
{
    public Publication(long id, DateTimeOffset publicationDate)
    {
        Id = id;
        PublicationDate = publicationDate;
    }

    public long Id { get; }
    public DateTimeOffset PublicationDate { get; private set; }

    public static Publication CreateTransient(DateTimeOffset publicationDate)
    {
        return new Publication(0, publicationDate);
    }
}
