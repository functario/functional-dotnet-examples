namespace Alexandria.Application.BookUseCases.GetBook;

public interface IGetBookService
{
    Task<GetBookResult?> HandleAsync(GetBookQuery query, CancellationToken cancellationToken);
}
