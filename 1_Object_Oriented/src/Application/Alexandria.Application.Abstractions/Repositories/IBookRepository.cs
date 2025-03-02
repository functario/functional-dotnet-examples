using Alexandria.Domain.BookDomain;

namespace Alexandria.Application.Abstractions.Repositories;

public interface IBookRepository
{
    Task<Func<Book>> CreateBookAsync(string title, CancellationToken cancellationToken);
    Task<Func<Publication>> CreatePublicationAsync(
        long bookId,
        DateTimeOffset publicationDate,
        ICollection<long> authorIds,
        CancellationToken cancellationToken
    );

    Task<Book?> GetBookAsync(long bookId, CancellationToken cancellationToken);
}
