namespace Alexandria.Application.AuthorUseCases.DeleteAuthor;

public interface IDeleteAuthorService
{
    Task<DeleteAuthorResult> HandleAsync(
        DeleteAuthorCommand command,
        CancellationToken cancellationToken
    );
}
