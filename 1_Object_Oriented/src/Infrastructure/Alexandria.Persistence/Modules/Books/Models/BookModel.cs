using System.ComponentModel.DataAnnotations;
using Alexandria.Domain.BookDomain;
using Alexandria.Persistence.Audits;

namespace Alexandria.Persistence.Modules.Books.Models;

internal class BookModel : IValidatableObject, IAuditable
{
    public long Id { get; set; }
    public required string Title { get; set; }
    public required PublicationModel Publication { get; set; }

    public ICollection<BookAuthorsModel> BookAuthors { get; set; } = [];

    public Book ToDomain()
    {
        var publication = Publication.ToDomain();
        var authorsIds = BookAuthors.Select(x => x.AuthorId);
        return new Book(Id, Title, publication, [.. authorsIds]);
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
    public static BookModel ToNewModel(this Book book)
    {
        return new BookModel()
        {
            Id = book.Id,
            Title = book.Title,
            Publication = book.Publication.ToNewModel(),
            BookAuthors = book
                .AuthorsIds.Select(authorId => new BookAuthorsModel
                {
                    BookId = book.Id,
                    AuthorId = authorId,
                })
                .ToList(),
        };
    }
}
