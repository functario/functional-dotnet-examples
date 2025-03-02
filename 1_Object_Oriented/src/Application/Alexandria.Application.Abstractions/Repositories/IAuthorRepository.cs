using Alexandria.Domain.AuthorDomain;

namespace Alexandria.Application.Abstractions.Repositories;

public interface IAuthorRepository : IRepository
{
    Task<Func<Author>> CreateAuthorAsync(Author author, CancellationToken cancellationToken);
    Task<Author?> GetAuthorAsync(long authorId, CancellationToken cancellationToken);
    Task<Author> DeleteAuthorAsync(long authorId, CancellationToken cancellationToken);
}
