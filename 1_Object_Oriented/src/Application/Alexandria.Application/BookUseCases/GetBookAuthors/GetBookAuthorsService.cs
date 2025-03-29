using Alexandria.Application.Abstractions.Repositories;

namespace Alexandria.Application.BookUseCases.GetBookAuthors;

internal sealed class GetBookAuthorsService : IGetBookAuthorsService
{
    private readonly IBookRepository _bookRepository;
    private readonly IAuthorRepository _authorRepository;

    public GetBookAuthorsService(IBookRepository bookRepository, IAuthorRepository authorRepository)
    {
        _bookRepository = bookRepository;
        _authorRepository = authorRepository;
    }

    public async Task<GetBookAuthorsResult?> Handle(
        GetBookAuthorsQuery query,
        CancellationToken cancellationToken
    )
    {
        var bookFound = await _bookRepository.GetBookAsync(query.BookId, cancellationToken);
        var authorsIds = bookFound?.AuthorsIds ?? [];
        var authors = await _authorRepository.FindAuthorsAsync(authorsIds, cancellationToken);
        return bookFound is not null ? new GetBookAuthorsResult(bookFound, authors) : null;
    }
}
