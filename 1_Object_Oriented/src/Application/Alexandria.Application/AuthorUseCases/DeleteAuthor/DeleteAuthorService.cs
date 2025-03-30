using Alexandria.Application.Abstractions.Repositories;

namespace Alexandria.Application.AuthorUseCases.DeleteAuthor;

internal sealed class DeleteAuthorService : IDeleteAuthorService
{
    private readonly IAuthorRepository _authorRepository;

    public DeleteAuthorService(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    public async Task<DeleteAuthorResult> HandleAsync(
        DeleteAuthorQuery query,
        CancellationToken cancellationToken
    )
    {
        var deletedAuthorId = await _authorRepository.DeleteAuthorAsync(
            query.AuhtorId,
            cancellationToken
        );
        return new DeleteAuthorResult(deletedAuthorId);
    }
}
