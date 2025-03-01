﻿using Alexandria.Application.Abstractions.Repositories;
using Alexandria.Domain.AuthorDomain;
using Alexandria.Persistence.Models;

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
        var result = await _alexandriaDbContext.AddAsync(author.AsAuthorModel(), cancellationToken);
        return result.Entity.ToAuthor;
    }

    public Task<Author> DeleteAuthorAsync(long authorId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Author> GetAuthorAsync(long authorId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
