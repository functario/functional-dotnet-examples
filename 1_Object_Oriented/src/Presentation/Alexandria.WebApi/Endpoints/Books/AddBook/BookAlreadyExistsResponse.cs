using Alexandria.Domain.BookDomain;

namespace Alexandria.WebApi.Endpoints.Books.AddBook;

internal sealed record BookAlreadyExistsResponse(Book Book)
{
    public string Message => $"{nameof(Book)} with Id '{Book.Id}' already exists.";
}
