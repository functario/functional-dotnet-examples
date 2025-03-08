namespace Alexandria.Persistence.Models;

internal class AuthorPublicationModel
{
    public long Id { get; set; }
    public long AuthorId { get; set; }
    public long PublicationId { get; set; }
}
