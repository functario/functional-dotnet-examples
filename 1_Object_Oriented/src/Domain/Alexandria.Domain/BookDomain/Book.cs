namespace Alexandria.Domain.BookDomain;

public class Book
{
    public Book(ulong id, string title, ulong authorId, DateTimeOffset publicationDate)
    {
        Id = id;
        Title = title;
        AuthorId = authorId;
        PublicationDate = publicationDate;
    }

    public ulong Id { get; }
    public string Title { get; }
    public ulong AuthorId { get; }
    public DateTimeOffset PublicationDate { get; }
}
