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

        // Publication is an Entity that map a book with its authors
        // Here the choice is made to create the publication in the same flow than than the book.
        // But it could be create by event (OnBookCreated) or by dedicated CreatePublication endpoint.
        // This is definatly something to revisit once the domain is more understood.
        // Domain rules should be:
        // - Publication cannot exist without Book and at least 1 Author
        transientPublication = transientPublication.AssociateBookId(book.Id);

        var publicationFunc = await _bookRepository.CreatePublicationAsync(
            transientPublication,
            cancellationToken
        );

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var publication = publicationFunc();

        var createdBook = new Book(book.Id, book.Title, publication);
        var response = new AddBookResult(createdBook);
        return response;
    }
}
