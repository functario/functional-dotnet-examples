using Alexandria.Domain.BookDomain;

namespace Alexandria.WebApi.Endpoints.Books.AddBook;

internal sealed record AddBookRequest(
    string Title,
    DateTimeOffset PublicationDate,
    ICollection<long> AuthorsIds
)
{
    internal Book ToCreatedBook()
    {
        var publication = new Publication(0, PublicationDate, AuthorsIds);
        return new Book(0, Title, publication);
    }
}
