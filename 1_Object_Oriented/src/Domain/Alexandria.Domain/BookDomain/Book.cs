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

    public static Book CreateTransient(string title, Publication transientPublication)
    {
        ArgumentNullException.ThrowIfNull(transientPublication, nameof(transientPublication));
        ArgumentException.ThrowIfNullOrWhiteSpace(title, nameof(title));

        var publicationTransient = Publication.CreateTransient(
            transientPublication.PublicationDate,
            transientPublication.AuthorsIds
        );

        return new Book(0, title, publicationTransient);
    }
}
