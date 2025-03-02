using Alexandria.Domain.AuthorDomain;

namespace Alexandria.WebApi.Endpoints.Authors.AddAuthor;

internal sealed record AddAuthorRequest(
    string FirstName,
    ICollection<string> MiddleNames,
    string LastName,
    DateTimeOffset BirthDate,
    ICollection<long> BooksIds
)
{
    internal Author ToAuthor()
    {
        // csharpier-ignore
        return new Author(
            0,
            FirstName,
            MiddleNames,
            LastName,
            BirthDate,
            BooksIds);
    }
}
