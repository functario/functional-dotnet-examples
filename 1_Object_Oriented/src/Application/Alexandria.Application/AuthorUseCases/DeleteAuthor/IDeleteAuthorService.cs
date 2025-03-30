namespace Alexandria.Application.AuthorUseCases.DeleteAuthor;

public interface IDeleteAuthorService
{
    Task<DeleteAuthorResult> HandleAsync(
        DeleteAuthorQuery query,
        CancellationToken cancellationToken
    );
}
