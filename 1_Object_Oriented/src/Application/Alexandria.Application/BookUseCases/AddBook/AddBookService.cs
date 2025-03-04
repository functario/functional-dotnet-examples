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

    public async Task<AddBookResult> Handle(
        AddBookCommand request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        var transientPublication = Publication.CreateTransient(
            request.PublicationDate,
            request.AuthorsIds
        );

        var transientBook = Book.CreateTransient(request.Title, transientPublication);

        var bookFunc = await _bookRepository.CreateBookAsync(transientBook, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var book = bookFunc();
        var response = new AddBookResult(book);
        return response;
    }
}
