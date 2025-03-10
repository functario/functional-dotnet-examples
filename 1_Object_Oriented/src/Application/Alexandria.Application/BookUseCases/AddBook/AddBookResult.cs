using Alexandria.Application.Abstractions.DTOs;

namespace Alexandria.Application.BookUseCases.AddBook;

public sealed record AddBookResult(BookDto Book) { }
