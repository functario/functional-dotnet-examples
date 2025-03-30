using Alexandria.Domain.BookDomain;
using Alexandria.Persistence.Audits;

namespace Alexandria.Persistence.Modules.Books.Models;

internal class PublicationModel : IAuditable
{
    public long Id { get; set; }
    public DateTimeOffset PublicationDate { get; set; }

    public Publication ToDomain()
    {
        return new Publication(Id, PublicationDate);
    }
}

internal static class PublicationModelExtensions
{
    public static PublicationModel ToNewModel(this Publication publication)
    {
        return new PublicationModel()
        {
            Id = publication.Id,
            PublicationDate = publication.PublicationDate,
        };
    }
}
