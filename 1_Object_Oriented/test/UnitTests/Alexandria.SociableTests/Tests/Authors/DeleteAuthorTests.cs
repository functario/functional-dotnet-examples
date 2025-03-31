using Alexandria.Application.Abstractions.Repositories;
using Alexandria.Application.AuthorUseCases.DeleteAuthor;
using Alexandria.Domain.BookDomain;
using Alexandria.SociableTests.Extensions;
using Alexandria.WebApi.Endpoints.Authors.DeleteAuthor;
using NSubstitute;
using VerifyDefault;

namespace Alexandria.SociableTests.Tests.Authors;

public class DeleteAuthorTests
{
    private readonly IDeleteAuthorEndpoint _deleteAuthorEndpoint;
    private readonly IDeleteAuthorService _deleteAuthorService;
    private readonly IAuthorRepository _authorRepository;
    private readonly IBookRepository _bookRepository;

    private readonly IUnitOfWork _unitOfWork;

    public DeleteAuthorTests(WebAppServicesFactory webAppServicesFactory)
    {
        (
            _deleteAuthorEndpoint,
            _deleteAuthorService,
            _authorRepository,
            _bookRepository,
            _unitOfWork
        ) = webAppServicesFactory.CreateServices<
            IDeleteAuthorEndpoint,
            IDeleteAuthorService,
            IAuthorRepository,
            IBookRepository,
            IUnitOfWork
        >();
    }

    [Theory(DisplayName = "Delete an existing Author by Id")]
    [AutoDataNSubstitute]
    internal async Task Test1(ICollection<Book> books)
    {
        // Arrange
        //_bookRepository.
        var authorId = 123;
        books = books
            .Select(b =>
            {
                // Ensure to have only the author to delete in these books
                b.AuthorsIds.Clear();
                b.AuthorsIds.Add(authorId);
                return b;
            })
            .ToArray();

        _authorRepository.DeleteAuthorAsync(authorId, CancellationToken.None).Returns(authorId);

        _bookRepository
            .GetManyBooksAuthorsAsync([authorId], CancellationToken.None)
            .ReturnsForAnyArgs(books.ToAsyncEnumerable());

        _bookRepository
            .DeleteManyBookAsync(Arg.Any<ICollection<long>>(), Arg.Any<CancellationToken>())
            .Returns(books.Count);

        _unitOfWork.SetExecuteTransactionAsync<DeleteAuthorResult>();

        // Act
        var sut = await _deleteAuthorEndpoint.HandleAsync(
            _deleteAuthorService,
            123,
            CancellationToken.None
        );

        // Assert
        await sut.VerifyHttpResponseAsync();
    }
}
