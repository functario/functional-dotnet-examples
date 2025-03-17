namespace Alexandria.Application.Abstractions.DTOs;

public sealed record BookDto(
    long Id,
    string Title,
    PublicationDto Publication,
    ICollection<AuthorDto> Authors
) { }
