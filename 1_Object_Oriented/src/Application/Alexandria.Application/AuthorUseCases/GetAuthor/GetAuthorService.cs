﻿using Alexandria.Application.Abstractions.Repositories;

namespace Alexandria.Application.AuthorUseCases.GetAuthor;

internal sealed class GetAuthorService : IGetAuthorService
{
    private readonly IAuthorRepository _authorRepository;

    public GetAuthorService(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    public async Task<GetAuthorResult?> Handle(
        GetAuthorQuery request,
        CancellationToken cancellationToken
    )
    {
        var authorFound = await _authorRepository.GetAuthorAsync(
            request.AuthorId,
            cancellationToken
        );

        return authorFound is not null ? new GetAuthorResult(authorFound) : null;
    }
}
