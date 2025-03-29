using Alexandria.Application.Abstractions.Repositories;
using Alexandria.Domain.AuthorDomain;
using Alexandria.Persistence.Authors.Models;

namespace Alexandria.Persistence.Repositories;

internal sealed class AuthorRepository : IAuthorRepository
{
    private readonly AlexandriaDbContext _alexandriaDbContext;
    private readonly TimeProvider _timeProvider;

    public AuthorRepository(AlexandriaDbContext alexandriaDbContext, TimeProvider timeProvider)
    {
        _alexandriaDbContext = alexandriaDbContext;
        _timeProvider = timeProvider;
    }

    public async Task<Func<Author>> CreateAuthorAsync(
        Author author,
        CancellationToken cancellationToken
    )
    {
        var creationDate = _timeProvider.GetUtcNow();
        var result = await _alexandriaDbContext.Authors.AddAsync(
            author.ToNewModel(creationDate),
            cancellationToken
        );

        return result.Entity.ToDomain;
    }

    public Task<Author> DeleteAuthorAsync(long authorId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<Author?> GetAuthorAsync(long authorId, CancellationToken cancellationToken)
    {
        var result = await _alexandriaDbContext.FindAsync<AuthorModel>(
            [authorId],
            cancellationToken
        );

        return result?.ToDomain();
    }

    public async Task<List<Author>> FindAuthorsAsync(
        ICollection<long> authorIds,
        CancellationToken cancellationToken
    )
    {
        var list = new List<Author>();
        foreach (var authorId in authorIds)
        {
            var result =
                await _alexandriaDbContext.FindAsync<AuthorModel>([authorId], cancellationToken)
                ?? throw new InvalidOperationException();
            list.Add(result.ToDomain());
        }

        return list;
    }
}
