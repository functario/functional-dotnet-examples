namespace Alexandria.Persistence.Models;

internal class AuthorsBooks
{
    public const string TableName = "AuthorsBooks";
    public long AuthorsId { get; set; }
    public long BooksId { get; set; }
}
