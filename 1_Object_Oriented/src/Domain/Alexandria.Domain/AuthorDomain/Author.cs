namespace Alexandria.Domain.AuthorDomain;

public class Author
{
    public Author(
        long id,
        string firstName,
        ICollection<string> middleNames,
        string lastName,
        DateTimeOffset birthDate,
        ICollection<long> booksIds
    )
    {
        Id = id;
        FirstName = firstName;
        MiddleNames = middleNames;
        LastName = lastName;
        BirthDate = birthDate;
        BooksIds = booksIds;
    }

    public long Id { get; }
    public string FirstName { get; }
    public ICollection<string> MiddleNames { get; }
    public string LastName { get; }
    public DateTimeOffset BirthDate { get; }
    public ICollection<long> BooksIds { get; }
}
