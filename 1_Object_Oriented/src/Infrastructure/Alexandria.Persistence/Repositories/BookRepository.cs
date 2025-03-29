using Alexandria.Application.Abstractions.Repositories;
using Alexandria.Domain.BookDomain;
using Alexandria.Persistence.Books.Models;
using Microsoft.EntityFrameworkCore;

namespace Alexandria.Persistence.Repositories;

internal sealed class BookRepository : IBookRepository
{
    private readonly AlexandriaDbContext _alexandriaDbContext;
    private readonly TimeProvider _timeProvider;

    public BookRepository(AlexandriaDbContext alexandriaDbContext, TimeProvider timeProvider)
    {
        _alexandriaDbContext = alexandriaDbContext;
        _timeProvider = timeProvider;
    }

    public async Task<Func<Book>> CreateBookAsync(Book book, CancellationToken cancellationToken)
    {
        var creationDate = _timeProvider.GetUtcNow();
        var result = await _alexandriaDbContext.Books.AddAsync(
            book.ToNewModel(creationDate),
            cancellationToken
        );

        return result.Entity.ToDomain;
    }

    public async Task<Book?> GetBookAsync(long bookId, CancellationToken cancellationToken)
    {
        var result = await _alexandriaDbContext
            .Books.Include(b => b.BookAuthors)
            .Include(b => b.Publication)
            .FirstOrDefaultAsync(b => b.Id == bookId, cancellationToken);

        return result?.ToDomain();
    }
}
