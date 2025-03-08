﻿using Alexandria.Application.Abstractions.DTOs;
using Alexandria.Domain.BookDomain;

namespace Alexandria.Application.Abstractions.Repositories;

public interface IBookRepository : IRepository
{
    Task<Func<BookDto>> CreateBookDtoAsync(Book book, CancellationToken cancellationToken);

    Task<BookDto?> GetBookDtoAsync(long bookId, CancellationToken cancellationToken);
}
