namespace Alexandria.Persistence.Models;

internal class BookAuthor
{
    public long Id { get; set; }
    public long BookId { get; set; }
    public long AuthorId { get; set; }
}
