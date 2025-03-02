using Alexandria.Domain.BookDomain;

namespace Alexandria.Persistence.Models;

internal class BookModel
{
    public long Id { get; init; }
    public required string Title { get; init; }
    public required PublicationModel Publication { get; init; }
    public required DateTimeOffset CreatedDate { get; init; }
    public required DateTimeOffset UpdatedDate { get; init; }

    public Book ToBook()
    {
        return new Book(Id, Title, Publication.ToDomainPublication());
    }
}

internal static class BookExtensions
{
    public static BookModel AsNewBookModel(this Book book, DateTimeOffset createdDate)
    {
        return new BookModel()
        {
            Id = book.Id,
            Title = book.Title,
            Publication = book.Publication.AsNewPublicationModel(createdDate),
            CreatedDate = createdDate,
            UpdatedDate = createdDate,
        };
    }
}
