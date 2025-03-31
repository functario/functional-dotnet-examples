namespace Alexandria.Domain.BookDomain;

public class Book
{
    public Book(
        long id,
        string title,
        string isbn,
        Publication publication,
        ICollection<long> authorsIds
    )
    {
        Id = id;
        Isbn = isbn;
        Title = title;
        Publication = publication;
        this.AuthorsIds = authorsIds;
    }

    public long Id { get; }
    public string Isbn { get; }
    public string Title { get; }
    public Publication Publication { get; }
    public ICollection<long> AuthorsIds { get; }

    public static Book CreateTransient(
        string title,
        string isbn,
        Publication transientPublication,
        ICollection<long> authorsIds
    )
    {
        ArgumentNullException.ThrowIfNull(transientPublication, nameof(transientPublication));
        ArgumentException.ThrowIfNullOrWhiteSpace(title, nameof(title));
        ArgumentNullException.ThrowIfNull(authorsIds, nameof(authorsIds));

        if (authorsIds.Count == 0)
        {
            throw new ArgumentException($"{authorsIds} cannot be empty", nameof(authorsIds));
        }

        var publicationTransient = Publication.CreateTransient(
            transientPublication.PublicationDate
        );

        return new Book(0, title, isbn, publicationTransient, authorsIds);
    }
}
