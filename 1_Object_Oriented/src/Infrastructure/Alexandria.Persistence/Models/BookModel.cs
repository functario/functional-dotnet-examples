using Alexandria.Application.Abstractions.DTOs;
using Alexandria.Domain.BookDomain;

namespace Alexandria.Persistence.Models;

internal class BookModel
{
    public long Id { get; set; }
    public required string Title { get; set; }
    public required DateTimeOffset CreatedDate { get; set; }
    public required DateTimeOffset UpdatedDate { get; set; }
    public required PublicationModel Publication { get; set; }

    public Book ToDomainBook(PublicationModel publication)
    {
        return new Book(Id, Title, publication.ToDomainPublication());
    }

    public BookDto ToBookDto(PublicationModel publication)
    {
        return new BookDto(Id, Title, publication.ToPublicationDto());
    }

    public Book ToNewDomainBook()
    {
        var publication = Publication.ToDomainPublication();
        return new Book(Id, Title, publication);
    }

    public BookDto ToNewBookDto()
    {
        var publication = Publication.ToPublicationDto();
        return new BookDto(Id, Title, publication);
    }
}
