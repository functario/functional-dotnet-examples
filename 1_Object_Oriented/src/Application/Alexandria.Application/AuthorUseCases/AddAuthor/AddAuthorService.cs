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
        AddAuthorCommand request,
        CancellationToken cancellationToken
    )
    {
        var author = request?.Author;
        ArgumentNullException.ThrowIfNull(author, nameof(request.Author));
        var createdAuthorFunc = await _authorRepository.CreateAuthorAsync(
            author,
            cancellationToken
        );
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        var createdAuthor = createdAuthorFunc();
        var response = new AddAuthorResult(createdAuthor);
        return response;
    }
}
