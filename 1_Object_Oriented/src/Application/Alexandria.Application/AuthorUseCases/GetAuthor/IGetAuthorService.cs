namespace Alexandria.Application.AuthorUseCases.GetAuthor;

public interface IGetAuthorService
{
    Task<GetAuthorResult?> HandleAsync(GetAuthorQuery query, CancellationToken cancellationToken);
}
