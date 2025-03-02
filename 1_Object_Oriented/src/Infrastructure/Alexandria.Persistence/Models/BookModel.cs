using Alexandria.Domain.BookDomain;

namespace Alexandria.Persistence.Models;

internal class BookModel
{
    public long Id { get; init; }
    public required string Title { get; init; }
    public ICollection<long> AuthorsIds { get; init; } = [];
    public DateTimeOffset PublicationDate { get; init; }

    public Book ToBook()
    {
        return new Book(Id, Title, AuthorsIds, PublicationDate);
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
            AuthorsIds = book.AuthorsIds,
            PublicationDate = createdDate,
        };
    }
}
