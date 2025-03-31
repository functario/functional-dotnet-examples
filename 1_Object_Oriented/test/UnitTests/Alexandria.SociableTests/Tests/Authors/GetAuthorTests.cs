using Alexandria.Application.Abstractions.Repositories;
using Alexandria.Application.AuthorUseCases.GetAuthor;
using Alexandria.Domain.AuthorDomain;
using Alexandria.WebApi.Endpoints.Authors.GetAuthor;
using NSubstitute;
using TestDefinitions.Traits;
using VerifyDefault;

namespace Alexandria.SociableTests.Tests.Authors;

[Trait(SizeTraits.Size, SizeTraits.S)]
[Trait(DomainTraits.Domain, DomainTraits.Author)]
public class GetAuthorTests
{
    private readonly IGetAuthorEndpoint _getAuthorEntpoint;
    private readonly IGetAuthorService _getAuthorService;
    private readonly IAuthorRepository _authorRepository;

    public GetAuthorTests(WebAppServicesFactory webAppServicesFactory)
    {
        (_getAuthorEntpoint, _getAuthorService, _authorRepository) =
            webAppServicesFactory.CreateServices<
                IGetAuthorEndpoint,
                IGetAuthorService,
                IAuthorRepository
            >();
    }

    [Fact(DisplayName = "Get a existing Author by Id")]
    public async Task Test1()
    {
        // Arrange
        var author = new Author(
            0,
            "firstname",
            ["A", "B"],
            "lastName",
            DateTimeOffset.FromUnixTimeSeconds(1234)
        );

        _authorRepository.GetAuthorAsync(author.Id, CancellationToken.None).Returns(author);

        // Act
        var sut = await _getAuthorEntpoint.HandleAsync(
            _getAuthorService,
            author.Id,
            CancellationToken.None
        );

        // Assert
        await sut.VerifyHttpResponseAsync();
    }
}
