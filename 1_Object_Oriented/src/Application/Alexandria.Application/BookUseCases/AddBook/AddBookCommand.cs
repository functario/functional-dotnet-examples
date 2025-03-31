namespace Alexandria.Application.BookUseCases.AddBook;

public sealed record AddBookCommand(
    string Title,
    string Isbn,
    DateTimeOffset PublicationDate,
    ICollection<long> AuthorsIds
) { }
