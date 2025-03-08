using Alexandria.Application.Abstractions.DTOs;

namespace Alexandria.Application.BookUseCases.GetBook;

public sealed record GetBookResult(BookDto Book) { }
