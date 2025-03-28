namespace Alexandria.Application.BookUseCases.AddBook;

public interface IAddBookService
{
    Task<AddBookResult> HandleAsync(AddBookCommand request, CancellationToken cancellationToken);
}
