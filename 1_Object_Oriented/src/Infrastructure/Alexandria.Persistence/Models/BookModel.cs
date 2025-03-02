using Alexandria.Domain.BookDomain;

namespace Alexandria.Persistence.Models;

internal class BookModel
{
    public long Id { get; init; }
    public required string Title { get; init; }
    public required DateTimeOffset CreatedDate { get; init; }
    public required DateTimeOffset UpdatedDate { get; init; }

    public Book ToDomainBook(PublicationModel publication)
    {
        return new Book(Id, Title, publication.ToDomainPublication());
    }

    public Book ToNewDomainBook()
    {
        var transientPublication = new Publication(0, Id, default, []);

        return new Book(Id, Title, transientPublication);
    }
}

internal static class BookExtensions
{
    public static BookModel AsNewBookModel(this Book book, DateTimeOffset createdDate)
    {
        return new BookModel()
        {
            Id = book.Id,
            Title = book.Title,
            CreatedDate = createdDate,
            UpdatedDate = createdDate,
        };
    }
}
