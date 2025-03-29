namespace Alexandria.Application.AuthorUseCases.GetAuthor;

public interface IGetAuthorService
{
    Task<GetAuthorResult?> Handle(GetAuthorQuery query, CancellationToken cancellationToken);
}
