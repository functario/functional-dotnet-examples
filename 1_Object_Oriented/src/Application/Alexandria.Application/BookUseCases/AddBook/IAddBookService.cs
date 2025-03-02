namespace Alexandria.Application.BookUseCases.AddBook;

public interface IAddBookService
{
    Task<AddBookResult> Handle(AddBookCommand request, CancellationToken cancellationToken);
}
