using Alexandria.Application.Abstractions.DTOs;
using Alexandria.Domain.AuthorDomain;

namespace Alexandria.Persistence.Models;

internal class AuthorModel
{
    public long Id { get; set; }
    public required string FirstName { get; set; }
    public ICollection<string> MiddleNames { get; set; } = [];
    public required string LastName { get; set; }
    public DateTimeOffset BirthDate { get; set; }
    public required DateTimeOffset? CreatedDate { get; set; }
    public required DateTimeOffset? UpdatedDate { get; set; }
    public virtual ICollection<BookModel>? Books { get; }

    public Author ToDomain()
    {
        return new Author(Id, FirstName, MiddleNames, LastName, BirthDate);
    }

    public AuthorDto ToDto()
    {
        return new AuthorDto(Id, FirstName, MiddleNames, LastName, BirthDate);
    }
}

internal static class AuthorExtensions
{
    public static AuthorModel ToNewModel(this Author author, DateTimeOffset createdDate)
    {
        return new AuthorModel()
        {
            Id = author.Id,
            FirstName = author.FirstName,
            MiddleNames = author.MiddleNames,
            LastName = author.LastName,
            BirthDate = author.BirthDate,
            CreatedDate = createdDate,
            UpdatedDate = createdDate,
        };
    }
}
