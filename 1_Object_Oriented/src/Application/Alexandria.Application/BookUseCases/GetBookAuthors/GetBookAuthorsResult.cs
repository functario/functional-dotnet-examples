using Alexandria.Domain.AuthorDomain;
using Alexandria.Domain.BookDomain;

namespace Alexandria.Application.BookUseCases.GetBookAuthors;

public sealed record GetBookAuthorsResult(Book Book, ICollection<Author> Authors) { }
