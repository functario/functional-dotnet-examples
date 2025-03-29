using Alexandria.Application.Abstractions.Repositories;
using Alexandria.Domain.BookDomain;

namespace Alexandria.Application.BookUseCases.AddBook;

public sealed class AddBookService : IAddBookService
{
    private readonly IBookRepository _bookRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddBookService(IBookRepository bookRepository, IUnitOfWork unitOfWork)
    {
        _bookRepository = bookRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<AddBookResult> HandleAsync(
        AddBookCommand command,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(command, nameof(command));
        var transientPublication = Publication.CreateTransient(command.PublicationDate);

        async Task<AddBookResult> Transaction(IUnitOfWork unitOfWork, CancellationToken ct)
        {
            // Create Book and related Publication
            var transientBook = Book.CreateTransient(
                command.Title,
                transientPublication,
                command.AuthorsIds
            );

            var bookTracker = await _bookRepository.CreateBookAsync(
                transientBook,
                cancellationToken
            );

            await unitOfWork.SaveChangesAsync(cancellationToken);

            var response = new AddBookResult(bookTracker());
            return response;
        }

        return await _unitOfWork.ExecuteTransactionAsync(Transaction, cancellationToken);
    }
}
