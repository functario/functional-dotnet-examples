using Alexandria.Domain.BookDomain;

namespace Alexandria.WebApi.Endpoints.Books.AddBook;

internal sealed record AddBookRequest(
    string Title,
    string Isbn,
    DateTimeOffset PublicationDate,
    ICollection<long> AuthorsIds
)
{
    internal Book ToCreatedBook()
    {
        var publication = new Publication(0, PublicationDate);
        return new Book(0, Title, Isbn, publication, AuthorsIds);
    }
}
