using Alexandria.Application.Abstractions.Repositories.Exceptions;
using Alexandria.Application.BookUseCases.AddBook;
using Alexandria.WebApi.Endpoints.Books.GetBook;
using Alexandria.WebApi.Supports;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Alexandria.WebApi.Endpoints.Books.AddBook;

internal sealed class AddBookEndpoint : IAddBookEndpoint
{
    public const string EndpointName = "PostBook";

    public void Map(IEndpointRouteBuilder endpointBuilder)
    {
        endpointBuilder
            .MapPost("/", HandleAsync)
            .WithSummary($"Add a Book.")
            .WithName(EndpointName);
    }

    public async Task<
        Results<
            Created<AddBookResponse>,
            NotFound<AuthorNotFoundResponse>,
            Conflict<BookAlreadyExistsResponse>
        >
    > HandleAsync(
        [FromServices] IAddBookService addBookService,
        LinkGenerator linkGenerator,
        HttpContext httpContext,
        [FromBody] AddBookRequest request,
        CancellationToken cancellationToken
    )
    {
        var command = new AddBookCommand(
            request.Title,
            request.Isbn,
            request.PublicationDate,
            request.AuthorsIds
        );
        try
        {
            var response = await addBookService.HandleAsync(command, cancellationToken);
            var result = new AddBookResponse(response.Book);
            var uri = linkGenerator.GetLocationUri(
                httpContext,
                GetBookEndpoint.EndpointName,
                GetBookEndpoint.QueryObjectValue(result.Book)
            )!;

            return TypedResults.Created(uri, result);
        }
        catch (EntityNotFoundException e)
        {
            return TypedResults.NotFound(new AuthorNotFoundResponse(request.ToCreatedBook(), e.Id));
        }
        catch (AuthorAlreadyExistsException)
        {
            return TypedResults.Conflict(new BookAlreadyExistsResponse(request.ToCreatedBook()));
        }
    }
}
