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
        var bookFunc = await _bookRepository.CreateBookAsync(request.Title, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var bookTransient = bookFunc();

        var publicationFunc = await _bookRepository.CreatePublicationAsync(
            bookTransient.Id,
            request.PublicationDate,
            request.AuthorsIds,
            cancellationToken
        );

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var publication = publicationFunc();

        var createdBook = new Book(bookTransient.Id, bookTransient.Title, publication);
        var response = new AddBookResult(createdBook);
        return response;
    }
}
