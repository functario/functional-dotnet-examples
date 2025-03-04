using System.ComponentModel.DataAnnotations;
using Alexandria.Domain.BookDomain;

namespace Alexandria.Persistence.Models;

internal class PublicationModel : IValidatableObject
{
    public long Id { get; set; }
    public long BookId { get; set; }
    public DateTimeOffset PublicationDate { get; set; }
    public ICollection<long> AuthorsIds { get; set; } = [];
    public required DateTimeOffset CreatedDate { get; set; }
    public required DateTimeOffset UpdatedDate { get; set; }

    public Publication ToDomainPublication()
    {
        return new Publication(Id, BookId, PublicationDate, AuthorsIds);
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (AuthorsIds == null || AuthorsIds.Count == 0)
        {
            yield return new ValidationResult(
                "A publication must have at least one author.",
                [nameof(AuthorsIds)]
            );
        }
    }
}

internal static class PublicationModelExtensions
{
    public static PublicationModel AsNewPublicationModel(
        this Publication publication,
        DateTimeOffset createdDate
    )
    {
        return new PublicationModel()
        {
            Id = publication.Id,
            AuthorsIds = publication.AuthorsIds,
            CreatedDate = createdDate,
            UpdatedDate = createdDate,
        };
    }
}
