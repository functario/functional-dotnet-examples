using Alexandria.Application.Abstractions.Repositories;
using Alexandria.Domain.AuthorDomain;

namespace Alexandria.Persistence.Repositories;

internal sealed class AuthorRepository : IAuthorRepository
{
    private readonly AlexandriaDbContext _alexandriaDbContext;

    public AuthorRepository(AlexandriaDbContext alexandriaDbContext)
    {
        _alexandriaDbContext = alexandriaDbContext;
    }

    public async Task<Author> CreateAuthor(Author author, CancellationToken cancellationToken)
    {
        var result = await _alexandriaDbContext.AddAsync(author, cancellationToken);
        return result.Entity;
    }

    public Task<Author> DeleteAuthor(long authorId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Author> GetAuthor(long authorId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
