using Alexandria.Application.AuthorUseCases.GetAuthor;
using Alexandria.Domain.AuthorDomain;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Alexandria.WebApi.Endpoints.Authors.GetAuthor;

internal sealed class GetAuthorEndpoint : IGetAuthorEndpoint
{
    public const string GetAuthorName = "GetAuthor";

    public void Map(IEndpointRouteBuilder endpointBuilder)
    {
        endpointBuilder
            .MapGet("/", HandleAsync)
            .WithSummary($"Get an Author.")
            .WithName(GetAuthorName);
    }

    public async Task<Results<Ok<GetAuthorResponse>, NotFound>> HandleAsync(
        [FromServices] IGetAuthorService getAuthorService,
        [FromQuery] long id,
        CancellationToken cancellationToken
    )
    {
        var query = new GetAuthorQuery(id);
        var response = await getAuthorService.Handle(query, cancellationToken);
        if (response is not null)
        {
            var result = new GetAuthorResponse(response.Author);
            return TypedResults.Ok(result);
        }

        return TypedResults.NotFound();
    }

    /// <summary>
    /// Returns a query object value to use with <see cref="LinkGenerator"/>
    /// to create the location URI.
    /// </summary>
    /// <param name="author">The author to query.</param>
    /// <returns>The query object value.</returns>
    internal static object QueryObjectValue(Author author) => new { id = author.Id };
}
