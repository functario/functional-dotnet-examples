using Alexandria.Application.Abstractions.Repositories;

namespace Alexandria.Application.BookUseCases.DeleteBook;

internal sealed class DeleteBookService : IDeleteBookService
{
    private readonly IBookRepository _bookRepository;

    public DeleteBookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<DeleteBookResult> HandleAsync(
        DeleteBookQuery query,
        CancellationToken cancellationToken
    )
    {
        var deletedBookId = await _bookRepository.DeleteBookAsync(query.BookId, cancellationToken);
        return new DeleteBookResult(deletedBookId);
    }
}
