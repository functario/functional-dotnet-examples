namespace Alexandria.Application.BookUseCases.AddBook;

public interface IAddBookService
{
    Task<AddBookResult> HandleAsync(AddBookCommand command, CancellationToken cancellationToken);
}
