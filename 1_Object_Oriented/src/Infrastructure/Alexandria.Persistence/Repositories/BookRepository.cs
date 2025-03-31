using System.Runtime.CompilerServices;
using Alexandria.Application.Abstractions.Repositories;
using Alexandria.Application.Abstractions.Repositories.Exceptions;
using Alexandria.Domain.BookDomain;
using Alexandria.Persistence.Modules.Books.Models;
using Microsoft.EntityFrameworkCore;

namespace Alexandria.Persistence.Repositories;

internal sealed class BookRepository : IBookRepository
{
    private readonly AlexandriaDbContext _alexandriaDbContext;

    public BookRepository(AlexandriaDbContext alexandriaDbContext)
    {
        _alexandriaDbContext = alexandriaDbContext;
    }

    public async Task<Func<Book>> CreateBookAsync(Book book, CancellationToken cancellationToken)
    {
        var result = await _alexandriaDbContext.Books.AddAsync(
            book.ToNewModel(),
            cancellationToken
        );

        return result.Entity.ToDomain;
    }

    // An example where EntityNotFoundException is used to indicated that the book was not found.
    public async Task<long> DeleteBookAsync(long bookId, CancellationToken cancellationToken)
    {
        var deletedRow = await _alexandriaDbContext
            .Books.Where(b => b.Id == bookId)
            .ExecuteDeleteAsync(cancellationToken);

        return deletedRow switch
        {
            0 => throw new EntityNotFoundException(bookId),
            1 => bookId,
            _ => throw new InvalidOperationException(),
        };
    }

    // TODO:
    // There is inconsistence
    // with the returns being number of rows
    // but the deleted Id in DeleteBookAsync
    public async Task<long> DeleteManyBookAsync(
        ICollection<long> booksIds,
        CancellationToken cancellationToken
    )
    {
        var deletedRows = await _alexandriaDbContext
            .Books.Where(b => booksIds.Contains(b.Id))
            .ExecuteDeleteAsync(cancellationToken);

        return deletedRows switch
        {
            0 => throw new EntityNotFoundException(
                $"The BooksIds [{string.Join(',', booksIds)}] were not found."
            ),
            > 0 => deletedRows,
            _ => throw new InvalidOperationException(),
        };
    }

    // An example where nullable book is used to indicated that the book was not found.
    public async Task<Book?> GetBookAsync(long bookId, CancellationToken cancellationToken)
    {
        var result = await _alexandriaDbContext
            .Books.Include(b => b.BookAuthors)
            .Include(b => b.Publication)
            .FirstOrDefaultAsync(b => b.Id == bookId, cancellationToken);

        return result?.ToDomain();
    }

    // TODO: Validate performance.
    // There is no reason to return IAsyncEnumerable here
    // since there is no real Async.
    // But method keep in example until a real case can be used.
    public async IAsyncEnumerable<Book> GetManyBooksAuthorsAsync(
        ICollection<long> authorIds,
        [EnumeratorCancellation] CancellationToken cancellationToken
    )
    {
        var query = _alexandriaDbContext
            .Books.Include(b => b.BookAuthors)
            .Include(b => b.Publication)
            .Where(x => x.BookAuthors.Any(a => authorIds.Contains(a.AuthorId)))
            .Select(x => x.ToDomain())
            .AsAsyncEnumerable();

        await foreach (var book in query.WithCancellation(cancellationToken))
        {
            yield return book;
        }
    }
}
