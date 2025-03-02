namespace Alexandria.Application.AuthorUseCases.AddAuthor;

public interface IAddAuthorService
{
    Task<AddAuthorResult> Handle(AddAuthorCommand request, CancellationToken cancellationToken);
}
