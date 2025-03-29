using Alexandria.Domain.AuthorDomain;
using Alexandria.Domain.BookDomain;

namespace Alexandria.WebApi.Endpoints.Books.GetBookAuthors;

internal sealed record GetBookAuthorsResponse(Book Book, ICollection<Author> Authors) { }
