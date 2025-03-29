using System.ComponentModel.DataAnnotations;
using Alexandria.Domain.BookDomain;

namespace Alexandria.Persistence.Models;

internal class BookModel : IValidatableObject
{
    public long Id { get; set; }
    public required string Title { get; set; }
    public required DateTimeOffset CreatedDate { get; set; }
    public required DateTimeOffset UpdatedDate { get; set; }
    public required PublicationModel Publication { get; set; }

    public ICollection<BookAuthors> BookAuthors { get; set; } = [];

    public Book ToDomain()
    {
        var authorsIds = BookAuthors.Where(x => x.BookId == Id).Select(x => x.AuthorId);
        return new Book(Id, Title, Publication.ToDomain(), [.. authorsIds]);
    }

    public Book ToNewDomain()
    {
        var publication = Publication.ToDomain();
        var authorsIds = BookAuthors.Where(x => x.BookId == Id).Select(x => x.AuthorId);
        return new Book(Id, Title, publication, [.. authorsIds]);
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (BookAuthors == null || BookAuthors.Count == 0)
        {
            yield return new ValidationResult(
                $"A {nameof(BookModel)} must have at least one author.",
                [nameof(BookAuthors)]
            );
        }
    }
}

internal static class BookExtensions
{
    public static BookModel ToNewModel(this Book book, DateTimeOffset createdDate)
    {
        return new BookModel()
        {
            Id = book.Id,
            Title = book.Title,
            CreatedDate = createdDate,
            UpdatedDate = createdDate,
            Publication = book.Publication.ToNewModel(createdDate),
        };
    }
}
