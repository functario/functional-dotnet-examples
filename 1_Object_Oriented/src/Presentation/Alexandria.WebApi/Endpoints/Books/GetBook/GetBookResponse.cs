using Alexandria.Application.Abstractions.DTOs;

namespace Alexandria.WebApi.Endpoints.Books.GetBook;

internal sealed record GetBookResponse(BookDto Book) { }
