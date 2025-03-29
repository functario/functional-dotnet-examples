using Alexandria.Domain.BookDomain;

namespace Alexandria.Application.Abstractions.Repositories;

public interface IBookRepository : IRepository
{
    Task<Func<Book>> CreateBookAsync(Book book, CancellationToken cancellationToken);
    Task<Book?> GetBookAsync(long bookId, CancellationToken cancellationToken);
}
