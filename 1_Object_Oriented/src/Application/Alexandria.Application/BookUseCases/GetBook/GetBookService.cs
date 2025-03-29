﻿using Alexandria.Application.Abstractions.Repositories;

namespace Alexandria.Application.BookUseCases.GetBook;

internal sealed class GetBookService : IGetBookService
{
    private readonly IBookRepository _bookRepository;

    public GetBookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<GetBookResult?> Handle(
        GetBookQuery request,
        CancellationToken cancellationToken
    )
    {
        var bookFound = await _bookRepository.GetBookAsync(request.BookId, cancellationToken);
        return bookFound is not null ? new GetBookResult(bookFound) : null;
    }
}
