namespace Alexandria.Application.BookUseCases.AddBook;

public sealed record AddBookCommand(
    string Title,
    DateTimeOffset PublicationDate,
    ICollection<long> AuthorsIds
) { }
