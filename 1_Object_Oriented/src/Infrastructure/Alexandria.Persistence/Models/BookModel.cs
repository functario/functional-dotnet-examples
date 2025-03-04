using Alexandria.Domain.BookDomain;

namespace Alexandria.Persistence.Models;

internal class BookModel
{
    public long Id { get; set; }
    public required string Title { get; set; }
    public required DateTimeOffset CreatedDate { get; set; }
    public required DateTimeOffset UpdatedDate { get; set; }
    public PublicationModel? Publication { get; set; }

    public Book ToDomainBook(PublicationModel publication)
    {
        return new Book(Id, Title, publication.ToDomainPublication());
    }

    public Book ToNewDomainBook()
    {
        var transientPublication = new Publication(0, Id, default, []);

        return new Book(Id, Title, transientPublication);
    }
}
