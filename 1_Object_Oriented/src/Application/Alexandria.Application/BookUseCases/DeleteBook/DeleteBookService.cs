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
        DeleteBookCommand command,
        CancellationToken cancellationToken
    )
    {
        var deletedBookId = await _bookRepository.DeleteBookAsync(
            command.BookId,
            cancellationToken
        );
        return new DeleteBookResult(deletedBookId);
    }
}
