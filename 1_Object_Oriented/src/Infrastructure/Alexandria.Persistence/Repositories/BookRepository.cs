﻿using Alexandria.Application.Abstractions.Repositories;
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

    public async Task<Book?> GetBookAsync(long bookId, CancellationToken cancellationToken)
    {
        var result = await _alexandriaDbContext
            .Books.Include(b => b.BookAuthors)
            .Include(b => b.Publication)
            .FirstOrDefaultAsync(b => b.Id == bookId, cancellationToken);

        return result?.ToDomain();
    }
}
