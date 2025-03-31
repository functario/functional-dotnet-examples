using Alexandria.Application.Abstractions.Repositories;
using Alexandria.Application.AuthorUseCases.AddAuthor;
using Alexandria.SociableTests.Extensions;
using Alexandria.WebApi.Endpoints.Authors.AddAuthor;
using Alexandria.WebApi.Endpoints.Authors.GetAuthor;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using NSubstitute;
using VerifyDefault;

namespace Alexandria.SociableTests.Tests.Authors;

public class AddAuthorTests
{
    private readonly IAddAuthorEndpoint _addAuthorEndpoint;
    private readonly IAddAuthorService _addAuthorService;
    private readonly IAuthorRepository _authorRepository;

    //private readonly IUnitOfWork _unitOfWork;

    public AddAuthorTests(WebAppServicesFactory webAppServicesFactory)
    {
        (_addAuthorEndpoint, _addAuthorService, _authorRepository) =
            webAppServicesFactory.CreateServices<
                IAddAuthorEndpoint,
                IAddAuthorService,
                IAuthorRepository
            >();
    }

    [Theory(DisplayName = "Get a valid Author by Id")]
    [AutoDataNSubstitute]
    internal async Task Test1(LinkGenerator linkGenerator, HttpContext httpContext)
    {
        // Arrange
        var request = new AddAuthorRequest(
            "firstname",
            ["A", "B"],
            "lastName",
            DateTimeOffset.FromUnixTimeSeconds(1234)
        );

        var result = new AddAuthorResult(request.ToAuthor());
        linkGenerator.SetGetUriByName(
            httpContext,
            $"https://localhost:7027/v1/authors?id={result.Author.Id}"
        );

        var response = new GetAuthorResponse(request.ToAuthor());

        _authorRepository
            .CreateAuthorAsync(request.ToAuthor(), CancellationToken.None)
            .ReturnsForAnyArgs(request.ToAuthor);

        // Act
        var sut = await _addAuthorEndpoint.HandleAsync(
            _addAuthorService,
            linkGenerator,
            httpContext,
            request,
            CancellationToken.None
        );

        // Assert
        await sut.VerifyHttpResponseAsync();
    }
}
