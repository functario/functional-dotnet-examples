using Alexandria.Domain.BookDomain;

namespace Alexandria.Persistence.Models;

internal class PublicationModel
{
    public long Id { get; set; }
    public long BookId { get; set; }
    public DateTimeOffset PublicationDate { get; set; }
    public ICollection<long> AuthorsIds { get; init; } = [];
    public required DateTimeOffset CreatedDate { get; init; }
    public required DateTimeOffset UpdatedDate { get; init; }

    public Publication ToDomainPublication()
    {
        return new Publication(Id, BookId, PublicationDate, AuthorsIds);
    }
}

internal static class PublicationExtensions
{
    public static PublicationModel AsNewPublicationModel(
        this Publication publication,
        DateTimeOffset createdDate
    )
    {
        return new PublicationModel()
        {
            Id = publication.Id,
            BookId = publication.BookId,
            PublicationDate = publication.PublicationDate,
            AuthorsIds = publication.AuthorsIds,
            CreatedDate = createdDate,
            UpdatedDate = createdDate,
        };
    }
}
