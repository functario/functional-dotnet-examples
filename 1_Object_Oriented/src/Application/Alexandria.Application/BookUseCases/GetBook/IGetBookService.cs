namespace Alexandria.Application.BookUseCases.GetBook;

public interface IGetBookService
{
    Task<GetBookResult?> Handle(GetBookQuery query, CancellationToken cancellationToken);
}
