using System.ComponentModel.DataAnnotations;
using Alexandria.Domain.BookDomain;

namespace Alexandria.Persistence.Models;

internal class PublicationModel : IValidatableObject
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
