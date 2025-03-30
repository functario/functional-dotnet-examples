using Alexandria.Application.Abstractions.Repositories;
using Alexandria.Application.Abstractions.Repositories.Exceptions;
using Alexandria.Domain.AuthorDomain;
using Alexandria.Persistence.Modules.Authors.Models;
using Microsoft.EntityFrameworkCore;

namespace Alexandria.Persistence.Repositories;

internal sealed class AuthorRepository : IAuthorRepository
{
    private readonly AlexandriaDbContext _alexandriaDbContext;

    public AuthorRepository(AlexandriaDbContext alexandriaDbContext)
    {
        _alexandriaDbContext = alexandriaDbContext;
    }

    public async Task<Func<Author>> CreateAuthorAsync(
        Author author,
        CancellationToken cancellationToken
    )
    {
        var result = await _alexandriaDbContext.Authors.AddAsync(
            author.ToNewModel(),
            cancellationToken
        );

        return result.Entity.ToDomain;
    }

    public async Task<long> DeleteAuthorAsync(long authorId, CancellationToken cancellationToken)
    {
        var deletedRows = await _alexandriaDbContext
            .Authors.Where(b => b.Id == authorId)
            .ExecuteDeleteAsync(cancellationToken);

        return deletedRows switch
        {
            0 => throw new EntityNotFoundException(authorId),
            1 => authorId,
            _ => throw new InvalidOperationException(),
        };
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
