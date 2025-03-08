using Alexandria.Application.Abstractions.Repositories;

namespace Alexandria.Application.BookUseCases.GetBook;

internal sealed class GetBookService : IGetBookService
{
    private readonly IBookRepository _authorRepository;

    public GetBookService(IBookRepository bookRepository)
    {
        _authorRepository = bookRepository;
    }

    public async Task<GetBookResult?> Handle(
        GetBookQuery request,
        CancellationToken cancellationToken
    )
    {
        var bookFound = await _authorRepository.GetBookDtoAsync(request.BookId, cancellationToken);
        return bookFound is not null ? new GetBookResult(bookFound) : null;
    }
}
