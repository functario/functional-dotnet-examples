namespace Alexandria.Persistence.Models;

internal class BookAuthors
{
    public long Id { get; set; }
    public long BookId { get; set; }
    public BookModel Book { get; set; } = null!;
    public long AuthorId { get; set; }
}
