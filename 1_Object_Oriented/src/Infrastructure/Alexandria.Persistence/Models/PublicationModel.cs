using Alexandria.Domain.BookDomain;

namespace Alexandria.Persistence.Models;

internal class PublicationModel
{
    public long Id { get; init; }
    public long BookId { get; init; }
    public DateTimeOffset PublicationDate { get; init; }
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
        long bookId,
        DateTimeOffset createdDate
    )
    {
        return new PublicationModel()
        {
            Id = publication.Id,
            BookId = bookId,
            PublicationDate = publication.PublicationDate,
            AuthorsIds = publication.AuthorsIds,
            CreatedDate = createdDate,
            UpdatedDate = createdDate,
        };
    }
}
