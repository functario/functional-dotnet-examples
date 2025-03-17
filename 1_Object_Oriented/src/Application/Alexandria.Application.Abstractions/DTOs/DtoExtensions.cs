using Alexandria.Domain.AuthorDomain;
using Alexandria.Domain.BookDomain;

namespace Alexandria.Application.Abstractions.DTOs;

public static class DtoExtensions
{
    public static BookDto ToDto(this Book book, ICollection<Author> authors)
    {
        ArgumentNullException.ThrowIfNull(book, nameof(book));
        var authorDtos = authors?.Select(a => a.ToDto()).ToList() ?? [];
        return new BookDto(book.Id, book.Title, book.Publication.ToDto(), authorDtos);
    }

    public static PublicationDto ToDto(this Publication publication)
    {
        ArgumentNullException.ThrowIfNull(publication, nameof(publication));
        return new PublicationDto(publication.Id, publication.PublicationDate);
    }

    public static AuthorDto ToDto(this Author author)
    {
        ArgumentNullException.ThrowIfNull(author, nameof(author));
        return new AuthorDto(
            author.Id,
            author.FirstName,
            author.MiddleNames,
            author.LastName,
            author.BirthDate
        );
    }
}
