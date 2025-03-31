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

    public async Task<AddAuthorResult> Handle(
        AddAuthorCommand command,
        CancellationToken cancellationToken
    )
    {
        var author = command?.Author;
        ArgumentNullException.ThrowIfNull(author, nameof(command.Author));
        var getCreatedAuthor = await _authorRepository.CreateAuthorAsync(author, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        var createdAuthor = getCreatedAuthor();
        var result = new AddAuthorResult(createdAuthor);
        return result;
    }
}
