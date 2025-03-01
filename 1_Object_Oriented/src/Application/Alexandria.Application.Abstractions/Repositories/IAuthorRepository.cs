using Alexandria.Domain.AuthorDomain;

namespace Alexandria.Application.Abstractions.Repositories;

public interface IAuthorRepository
{
    Task<Author> CreateAuthor(Author author, CancellationToken cancellationToken);
    Task<Author> GetAuthor(long authorId, CancellationToken cancellationToken);
    Task<Author> DeleteAuthor(long authorId, CancellationToken cancellationToken);
}
