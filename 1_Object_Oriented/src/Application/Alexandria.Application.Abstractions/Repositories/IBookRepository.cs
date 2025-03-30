using Alexandria.Domain.BookDomain;

namespace Alexandria.Application.Abstractions.Repositories;

public interface IBookRepository : IRepository
{
    Task<Func<Book>> CreateBookAsync(Book book, CancellationToken cancellationToken);
    Task<Book?> GetBookAsync(long bookId, CancellationToken cancellationToken);
    IAsyncEnumerable<Book> GetManyBooksAuthorsAsync(
        ICollection<long> authorIds,
        CancellationToken cancellationToken
    );
    Task<long> DeleteBookAsync(long bookId, CancellationToken cancellationToken);
    Task<long> DeleteManyBookAsync(ICollection<long> booksIds, CancellationToken cancellationToken);
}
