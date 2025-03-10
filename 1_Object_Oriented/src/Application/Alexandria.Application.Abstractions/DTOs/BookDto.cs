using Alexandria.Domain.AuthorDomain;
using Alexandria.Domain.BookDomain;

namespace Alexandria.Application.Abstractions.DTOs;

public sealed record BookDto(long Id, string Title, PublicationDto Publication) { }

public sealed record PublicationDto(
    long Id,
    DateTimeOffset PublicationDate,
    ICollection<AuthorDto> Authors
) { }

public sealed record AuthorDto(
    long Id,
    string FirstName,
    ICollection<string> MiddleNames,
    string LastName,
    DateTimeOffset BirthDate
) { }

public static class BookDtoExtensions
{
    public static BookDto AsDto(this Book book, ICollection<Author> authors)
    {
        ArgumentNullException.ThrowIfNull(book, nameof(book));
        return new BookDto(book.Id, book.Title, book.Publication.AsDto(authors));
    }

    public static PublicationDto AsDto(this Publication publication, ICollection<Author> authors)
    {
        ArgumentNullException.ThrowIfNull(publication, nameof(publication));
        var authorDtos = authors.Select(x => x.AsDto()).ToList();
        return new PublicationDto(publication.Id, publication.PublicationDate, authorDtos);
    }

    public static AuthorDto AsDto(this Author author)
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
