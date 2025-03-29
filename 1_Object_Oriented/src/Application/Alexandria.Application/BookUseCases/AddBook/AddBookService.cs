using Alexandria.Application.Abstractions.Repositories;
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

    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Style",
        "IDE0059:Unnecessary assignment of a value",
        Justification = "<Pending>"
    )]
    public async Task<AddBookResult> HandleAsync(
        AddBookCommand request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        var transientPublication = Publication.CreateTransient(request.PublicationDate);

        async Task<AddBookResult> Transaction(IUnitOfWork unitOfWork, CancellationToken ct)
        {
            // Create Book and related Publication
            var transientBook = Book.CreateTransient(
                request.Title,
                transientPublication,
                request.AuthorsIds
            );
            var bookFunc = await _bookRepository.CreateBookAsync(transientBook, cancellationToken);

            // Note: The Many-to-Many relation between Publication and Author
            // is created via an overload of SaveChangeAsync.
            await unitOfWork.SaveChangesAsync(cancellationToken);

            // Get book with information from database.
            var book = bookFunc();

            // Create the response.
            var authors = await _authorRepository.FindAuthorsAsync(
                book.AuthorsIds,
                cancellationToken
            );

            var response = new AddBookResult(book);
            return response;
        }

        return await _unitOfWork.ExecuteTransactionAsync(Transaction, cancellationToken);
    }
}
