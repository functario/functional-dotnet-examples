namespace Alexandria.Application.AuthorUseCases.AddAuthor;

public interface IAddAuthorService
{
    Task<AddAuthorResult> HandleAsync(
        AddAuthorCommand command,
        CancellationToken cancellationToken
    );
}
