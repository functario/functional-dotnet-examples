using Alexandria.Application.Abstractions.Repositories;

namespace Alexandria.Application.BookUseCases.DeleteBook;

internal sealed class DeleteBookService : IDeleteBookService
{
    private readonly IBookRepository _bookRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBookService(IBookRepository bookRepository, IUnitOfWork unitOfWork)
    {
        _bookRepository = bookRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<DeleteBookResult> HandleAsync(
        DeleteBookCommand command,
        CancellationToken cancellationToken
    )
    {
        async Task<DeleteBookResult> Transaction(IUnitOfWork unitOfWork, CancellationToken ct)
        {
            var deletedBookId = await _bookRepository.DeleteBookAsync(
                command.BookId,
                cancellationToken
            );
            return new DeleteBookResult(deletedBookId);
        }

        return await _unitOfWork.ExecuteTransactionAsync(Transaction, cancellationToken);
    }
}
