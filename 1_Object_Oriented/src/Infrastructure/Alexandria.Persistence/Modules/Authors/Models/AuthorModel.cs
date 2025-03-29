using Alexandria.Domain.AuthorDomain;
using Alexandria.Persistence.Audits;

namespace Alexandria.Persistence.Modules.Authors.Models;

internal class AuthorModel : IAuditable
{
    public long Id { get; set; }
    public required string FirstName { get; set; }
    public ICollection<string> MiddleNames { get; set; } = [];
    public required string LastName { get; set; }
    public DateTimeOffset BirthDate { get; set; }

    public Author ToDomain()
    {
        return new Author(Id, FirstName, MiddleNames, LastName, BirthDate);
    }
}

internal static class AuthorExtensions
{
    public static AuthorModel ToNewModel(this Author author)
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
