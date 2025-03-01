using Alexandria.Application.Abstractions.Repositories;

namespace Alexandria.Application.AuthorUseCases.AddAuthor;

internal sealed class AddAuthorService : IAddAuthorService
{
    private readonly IAuthorRepository _authorRepository;

    public AddAuthorService(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    public async Task<AddAuthorResponse> Handle(
        AddAuthorCommand request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request?.Author, nameof(request.Author));
        var createdAuthor = await _authorRepository.CreateAuthor(request.Author, cancellationToken);
        var response = new AddAuthorResponse(createdAuthor);
        return response;
    }
}
