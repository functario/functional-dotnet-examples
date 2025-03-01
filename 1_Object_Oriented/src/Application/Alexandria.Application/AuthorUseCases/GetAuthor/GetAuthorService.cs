using Alexandria.Application.Abstractions.Repositories;

namespace Alexandria.Application.AuthorUseCases.GetAuthor;

internal sealed class GetAuthorService : IGetAuthorService
{
    private readonly IAuthorRepository _authorRepository;
    private readonly IUnitOfWork _unitOfWork;

    public GetAuthorService(IAuthorRepository authorRepository, IUnitOfWork unitOfWork)
    {
        _authorRepository = authorRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<GetAuthorResult?> Handle(
        GetAuthorQuery request,
        CancellationToken cancellationToken
    )
    {
        var authorFound = await _authorRepository.GetAuthorAsync(
            request.AuthorId,
            cancellationToken
        );

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return authorFound is not null ? new GetAuthorResult(authorFound) : null;
    }
}
