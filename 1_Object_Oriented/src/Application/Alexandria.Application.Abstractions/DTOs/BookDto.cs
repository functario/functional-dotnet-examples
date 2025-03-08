namespace Alexandria.Application.Abstractions.DTOs;

public sealed record BookDto(long Id, string Title, PublicationDto Publication) { }

public sealed record PublicationDto(
    long Id,
    DateTimeOffset PublicationDate,
    ICollection<AuthorDto> Authors
) { }

public sealed record AuthorDto(
    long Id,
    string FirstName,
    ICollection<string> MiddleNames,
    string LastName,
    DateTimeOffset BirthDate
) { }
