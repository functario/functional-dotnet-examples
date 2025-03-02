namespace Alexandria.Domain.BookDomain;

public class Book
{
    public Book(long id, string title, ICollection<long> authorsIds, DateTimeOffset publicationDate)
    {
        Id = id;
        Title = title;
        AuthorsIds = authorsIds;
        PublicationDate = publicationDate;
    }

    public long Id { get; }
    public string Title { get; }
    public ICollection<long> AuthorsIds { get; }
    public DateTimeOffset PublicationDate { get; }
}
