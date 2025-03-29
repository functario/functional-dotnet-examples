using Alexandria.Domain.BookDomain;

namespace Alexandria.Persistence.Books.Models;

internal class PublicationModel
{
    public long Id { get; set; }
    public DateTimeOffset PublicationDate { get; set; }
    public required DateTimeOffset CreatedDate { get; set; }
    public required DateTimeOffset UpdatedDate { get; set; }

    public Publication ToDomain()
    {
        return new Publication(Id, PublicationDate);
    }
}

internal static class PublicationModelExtensions
{
    public static PublicationModel ToNewModel(
        this Publication publication,
        DateTimeOffset createdDate
    )
    {
        return new PublicationModel()
        {
            Id = publication.Id,
            CreatedDate = createdDate,
            UpdatedDate = createdDate,
            PublicationDate = publication.PublicationDate,
        };
    }
}
