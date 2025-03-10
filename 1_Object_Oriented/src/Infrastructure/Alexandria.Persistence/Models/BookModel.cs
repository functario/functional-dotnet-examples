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

    public Book ToDomain(PublicationModel publication)
    {
        return new Book(Id, Title, publication.ToDomain());
    }

    public BookDto ToDto(PublicationModel publication)
    {
        return new BookDto(Id, Title, publication.ToDto());
    }

    public Book ToNewDomain()
    {
        var publication = Publication.ToDomain();
        return new Book(Id, Title, publication);
    }

    public BookDto ToNewDto()
    {
        var publication = Publication.ToDto();
        return new BookDto(Id, Title, publication);
    }
}
