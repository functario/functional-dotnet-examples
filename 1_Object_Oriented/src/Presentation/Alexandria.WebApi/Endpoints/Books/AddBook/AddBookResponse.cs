using Alexandria.Application.Abstractions.DTOs;

namespace Alexandria.WebApi.Endpoints.Books.AddBook;

internal sealed record AddBookResponse(BookDto Book) { }
