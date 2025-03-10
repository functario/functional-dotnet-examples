﻿using Alexandria.Application.Abstractions.DTOs;
using Alexandria.Application.Abstractions.Repositories;
using Alexandria.Domain.BookDomain;
using Alexandria.Persistence.Models;
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

        var bookModel = new BookModel()
        {
            Id = 0,
            Title = book.Title,
            CreatedDate = creationDate,
            UpdatedDate = creationDate,
            Publication = book.Publication.ToNewModel(creationDate),
        };

        var result = await _alexandriaDbContext.Books.AddAsync(bookModel, cancellationToken);

        var entry = _alexandriaDbContext.Entry(bookModel);
        var createdBookModel = result.Entity;

        return result.Entity.ToNewDomain;
    }

    public async Task<BookDto?> GetBookDtoAsync(long bookId, CancellationToken cancellationToken)
    {
        var bookModel = await _alexandriaDbContext.FindAsync<BookModel>(
            [bookId],
            cancellationToken
        );

        if (bookModel is null)
        {
            return null;
        }

        var publication = await _alexandriaDbContext
            .Publications.Include(x => x.Authors)
            .FirstOrDefaultAsync(x => x.Id == bookModel.Id, cancellationToken);

        return publication is null
            ? throw new InvalidOperationException(
                $"No {nameof(Publication)} match the Book with Id '{bookModel.Id}'"
            )
            : bookModel.ToDto(publication);
    }
}
