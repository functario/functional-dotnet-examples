using Alexandria.Domain.AuthorDomain;

namespace Alexandria.Persistence.Models;

internal class AuthorModel
{
    public long Id { get; init; }
    public required string FirstName { get; init; }
    public ICollection<string> MiddleNames { get; init; } = [];
    public required string LastName { get; init; }
    public DateTimeOffset BirthDate { get; init; }

    public Author ToAuthor()
    {
        return new Author(Id, FirstName, MiddleNames, LastName, BirthDate);
    }
}

internal static class AuthorExtensions
{
    public static AuthorModel AsAuthorModel(this Author author)
    {
        return new AuthorModel()
        {
            Id = author.Id,
            FirstName = author.FirstName,
            MiddleNames = author.MiddleNames,
            LastName = author.LastName,
            BirthDate = author.BirthDate,
        };
    }
}
