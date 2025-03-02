using Alexandria.Application.Abstractions.Repositories;
using Alexandria.Domain.BookDomain;
using Alexandria.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace Alexandria.Persistence.Repositories;

internal sealed class BookRepository : IBookRepository
{
    private readonly AlexandriaDbContext _alexandriaDbContext;
    private readonly TimeProvider _timeProvider;

    public BookRepository(AlexandriaDbContext alexandriaDbContext, TimeProvider timeProvider)
    {
        _alexandriaDbContext = alexandriaDbContext;
        _timeProvider = timeProvider;
    }

    public async Task<Func<Book>> CreateBookAsync(string title, CancellationToken cancellationToken)
    {
        var creationDate = _timeProvider.GetUtcNow();
        var book = new BookModel()
        {
            Id = 0,
            Title = title,
            CreatedDate = creationDate,
            UpdatedDate = creationDate,
        };

        var result = await _alexandriaDbContext.Books.AddAsync(book, cancellationToken);

        var createdBookModel = result.Entity;

        return result.Entity.ToNewDomainBook;
    }

    public async Task<Func<Book>> CreateBookAsync(Book book, CancellationToken cancellationToken)
    {
        var creationDate = _timeProvider.GetUtcNow();
        var bookModel = new BookModel()
        {
            Id = 0,
            Title = book.Title,
            CreatedDate = creationDate,
            UpdatedDate = creationDate,
        };

        var result = await _alexandriaDbContext.Books.AddAsync(bookModel, cancellationToken);

        var createdBookModel = result.Entity;

        return result.Entity.ToNewDomainBook;
    }

    public async Task<Func<Publication>> CreatePublicationAsync(
        Publication publication,
        CancellationToken cancellationToken
    )
    {
        var creationDate = _timeProvider.GetUtcNow();

        var publicationModel = new PublicationModel()
        {
            Id = 0,
            BookId = publication.BookId,
            PublicationDate = publication.PublicationDate,
            AuthorsIds = publication.AuthorsIds,
            CreatedDate = creationDate,
            UpdatedDate = creationDate,
        };

        var result = await _alexandriaDbContext.Publications.AddAsync(
            publicationModel,
            cancellationToken
        );

        return result.Entity.ToDomainPublication;
    }

    public async Task<Func<Publication>> CreatePublicationAsync(
        long bookId,
        DateTimeOffset publicationDate,
        ICollection<long> authorIds,
        CancellationToken cancellationToken
    )
    {
        var creationDate = _timeProvider.GetUtcNow();

        var publication = new PublicationModel()
        {
            Id = 0,
            BookId = bookId,
            PublicationDate = publicationDate,
            AuthorsIds = authorIds,
            CreatedDate = creationDate,
            UpdatedDate = creationDate,
        };

        var result = await _alexandriaDbContext.Publications.AddAsync(
            publication,
            cancellationToken
        );

        return result.Entity.ToDomainPublication;
    }

    public async Task<Book?> GetBookAsync(long bookId, CancellationToken cancellationToken)
    {
        var bookModel = await _alexandriaDbContext.FindAsync<BookModel>(
            [bookId],
            cancellationToken
        );

        if (bookModel is null)
        {
            return null;
        }

        var publication = await _alexandriaDbContext.Publications.FirstOrDefaultAsync(
            x => x.BookId == bookModel.Id,
            cancellationToken
        );

        return publication is null
            ? throw new InvalidOperationException(
                $"No {nameof(Publication)} match the Book with Id '{bookModel.Id}'"
            )
            : bookModel.ToDomainBook(publication);
    }
}
