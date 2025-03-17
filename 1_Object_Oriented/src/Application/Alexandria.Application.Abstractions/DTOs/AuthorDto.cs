namespace Alexandria.Application.Abstractions.DTOs;

public sealed record AuthorDto(
    long Id,
    string FirstName,
    ICollection<string> MiddleNames,
    string LastName,
    DateTimeOffset BirthDate
) { }
