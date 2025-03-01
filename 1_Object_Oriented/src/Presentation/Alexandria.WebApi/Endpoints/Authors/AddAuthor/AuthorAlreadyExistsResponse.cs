using Alexandria.Domain.AuthorDomain;

namespace Alexandria.WebApi.Endpoints.Authors.AddAuthor;

internal sealed record AuthorAlreadyExistsResponse(Author Author)
{
    public string Message => $"The Author '{Author.Id}' already exists.";
}
