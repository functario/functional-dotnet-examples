using Alexandria.Application.Abstractions.Repositories;

namespace Alexandria.Application.AuthorUseCases.DeleteAuthor;

internal sealed class DeleteAuthorService : IDeleteAuthorService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthorRepository _authorRepository;
    private readonly IBookRepository _bookRepository;

    public DeleteAuthorService(
        IUnitOfWork unitOfWork,
        IAuthorRepository authorRepository,
        IBookRepository bookRepository
    )
    {
        _unitOfWork = unitOfWork;
        _authorRepository = authorRepository;
        _bookRepository = bookRepository;
    }

    public async Task<DeleteAuthorResult> HandleAsync(
        DeleteAuthorCommand command,
        CancellationToken cancellationToken
    )
    {
        // Example of monolithic implementation where the deletion of an Author
        // directly handle the deletion of the corresponding book.
        // A better approach could be to use mediatr to keep the events sequences
        // but with handling abstraction.
        async Task<DeleteAuthorResult> Transaction(IUnitOfWork unitOfWork, CancellationToken ct)
        {
            // If a book contains only this Author it will be deleted.
            // Otherwise the caller must handle the book deletion before to Author ones.
            var booksIds = new List<long>();
            await foreach (
                var book in _bookRepository.GetManyBooksAuthorsAsync([command.AuhtorId], ct)
            )
            {
                if (book.AuthorsIds.Any(id => id != command.AuhtorId))
                {
                    throw new InvalidOperationException(
                        $"Cannot delete Author with Id '{command.AuhtorId}' "
                            + $"because the Book with Id '{book.Id}' contains other authors."
                    );
                }

                booksIds.Add(book.Id);
            }

            var deletedAuthorId = await _authorRepository.DeleteAuthorAsync(
                command.AuhtorId,
                cancellationToken
            );

            await unitOfWork.SaveChangesAsync(ct);

            await _bookRepository.DeleteManyBookAsync(booksIds, ct);
            await unitOfWork.SaveChangesAsync(ct);
            return new DeleteAuthorResult(deletedAuthorId);
        }

        return await _unitOfWork.ExecuteTransactionAsync(Transaction, cancellationToken);
    }
}
