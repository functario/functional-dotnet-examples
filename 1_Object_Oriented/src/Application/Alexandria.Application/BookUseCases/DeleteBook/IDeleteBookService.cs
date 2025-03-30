namespace Alexandria.Application.BookUseCases.DeleteBook;

public interface IDeleteBookService
{
    Task<DeleteBookResult> HandleAsync(DeleteBookQuery query, CancellationToken cancellationToken);
}
