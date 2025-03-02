namespace Alexandria.Domain.BookDomain;

public class Book
{
    public Book(long id, string title, Publication publication)
    {
        Id = id;
        Title = title;
        Publication = publication;
    }

    public long Id { get; }
    public string Title { get; }
    public Publication Publication { get; }
}
