namespace Alexandria.Application.BookUseCases.GetBook;

public interface IGetBookService
{
    Task<GetBookResult?> Handle(GetBookQuery request, CancellationToken cancellationToken);
}
