using Alexandria.Application.Abstractions.Repositories;

namespace Alexandria.Application.AuthorUseCases.AddAuthor;

internal sealed class AddAuthorService : IAddAuthorService
{
    private readonly IAuthorRepository _authorRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddAuthorService(IAuthorRepository authorRepository, IUnitOfWork unitOfWork)
    {
        _authorRepository = authorRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<AddAuthorResult> HandleAsync(
        AddAuthorCommand command,
        CancellationToken cancellationToken
    )
    {
        var author = command?.Author;
        ArgumentNullException.ThrowIfNull(author, nameof(command.Author));

        async Task<AddAuthorResult> TransactionAsync(IUnitOfWork unitOfWork, CancellationToken ct)
        {
            var getCreatedAuthor = await _authorRepository.CreateAuthorAsync(
                author,
                cancellationToken
            );

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            var createdAuthor = getCreatedAuthor();
            return new AddAuthorResult(createdAuthor);
        }

        return await _unitOfWork.ExecuteTransactionAsync(TransactionAsync, cancellationToken);
    }
}
