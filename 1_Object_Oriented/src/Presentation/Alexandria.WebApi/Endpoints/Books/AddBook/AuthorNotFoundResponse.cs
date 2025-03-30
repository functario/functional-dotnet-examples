using Alexandria.Domain.AuthorDomain;
using Alexandria.Domain.BookDomain;

namespace Alexandria.WebApi.Endpoints.Books.AddBook;

internal sealed record AuthorNotFoundResponse(Book Book, long AuthorId)
{
    public string Message =>
        $"The {nameof(Book)}'s {nameof(Author)} with AuthorId '{AuthorId}' was not found.";
}
