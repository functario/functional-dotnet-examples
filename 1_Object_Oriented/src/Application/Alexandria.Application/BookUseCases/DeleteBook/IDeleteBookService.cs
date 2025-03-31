namespace Alexandria.Application.BookUseCases.DeleteBook;

public interface IDeleteBookService
{
    Task<DeleteBookResult> HandleAsync(
        DeleteBookCommand command,
        CancellationToken cancellationToken
    );
}
