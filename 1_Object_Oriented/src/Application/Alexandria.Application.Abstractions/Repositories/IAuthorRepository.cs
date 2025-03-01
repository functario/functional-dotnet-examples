using Alexandria.Domain.AuthorDomain;

namespace Alexandria.Application.Abstractions.Repositories;

public interface IAuthorRepository
{
    Task<Author> CreateAuthor(Author author);
    Task<Author> GetAuthor(long authorId);
    Task<Author> DeleteAuthor(long authorId);
}
