using Alexandria.Application.Abstractions.Repositories;
using Alexandria.Domain.AuthorDomain;

namespace Alexandria.Persistence.Repositories;

public sealed class AuthorRepository : IAuthorRepository
{
    public Task<Author> CreateAuthor(Author author)
    {
        throw new NotImplementedException();
    }

    public Task<Author> DeleteAuthor(long authorId)
    {
        throw new NotImplementedException();
    }

    public Task<Author> GetAuthor(long authorId)
    {
        throw new NotImplementedException();
    }
}
