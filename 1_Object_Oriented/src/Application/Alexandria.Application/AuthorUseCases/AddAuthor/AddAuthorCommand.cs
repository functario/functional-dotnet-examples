using Alexandria.Domain.AuthorDomain;

namespace Alexandria.Application.AuthorUseCases.AddAuthor;

public sealed record AddAuthorCommand(Author Author) { }
