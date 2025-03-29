namespace Alexandria.Persistence.Modules.Books.Models;

internal class BookAuthorsModel
{
    public long Id { get; set; }
    public long BookId { get; set; }
    public BookModel Book { get; set; } = null!;
    public long AuthorId { get; set; }
}
