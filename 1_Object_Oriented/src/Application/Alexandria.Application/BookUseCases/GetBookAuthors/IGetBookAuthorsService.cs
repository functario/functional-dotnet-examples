namespace Alexandria.Application.BookUseCases.GetBookAuthors;

public interface IGetBookAuthorsService
{
    Task<GetBookAuthorsResult?> Handle(
        GetBookAuthorsQuery query,
        CancellationToken cancellationToken
    );
}
