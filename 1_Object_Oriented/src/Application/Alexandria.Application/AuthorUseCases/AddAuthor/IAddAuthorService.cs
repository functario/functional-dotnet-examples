namespace Alexandria.Application.AuthorUseCases.AddAuthor;

public interface IAddAuthorService
{
    Task<AddAuthorResponse> Handle(AddAuthorCommand request, CancellationToken _);
}
