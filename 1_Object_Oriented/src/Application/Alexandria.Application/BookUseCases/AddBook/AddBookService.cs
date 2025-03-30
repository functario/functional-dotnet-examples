using Alexandria.Application.Abstractions.Repositories;
using Alexandria.Application.Abstractions.Repositories.Exceptions;
using Alexandria.Domain.BookDomain;

namespace Alexandria.Application.BookUseCases.AddBook;

public sealed class AddBookService : IAddBookService
{
    private readonly IBookRepository _bookRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddBookService(
        IBookRepository bookRepository,
        IAuthorRepository authorRepository,
        IUnitOfWork unitOfWork
    )
    {
        _bookRepository = bookRepository;
        _authorRepository = authorRepository;
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
            // Example of monolithic implementation where we need to valide if
            // the Author exist before adding the book.
            // A better approach could be to use mediatr to keep the events sequences
            // but with handling abstraction.
            foreach (var authorId in command.AuthorsIds)
            {
                var author =
                    await _authorRepository.GetAuthorAsync(authorId, cancellationToken)
                    ?? throw new EntityNotFoundException(authorId);
            }

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
