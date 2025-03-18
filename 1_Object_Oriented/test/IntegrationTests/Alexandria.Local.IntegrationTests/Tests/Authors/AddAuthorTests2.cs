using System.Globalization;
using Alexandria.Local.IntegrationTests.Support;
using Aspire.Hosting;
using CleanArchitecture.WebAPI.Client;
using CleanArchitecture.WebAPI.Client.Models;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using WellKnowns.Aspires;
using WellKnowns.Presentation.AlexandriaWebApi;

namespace Alexandria.Local.IntegrationTests.Tests.Authors;

[Trait("Category", "Aspire")]
[Collection(nameof(IntegratedTests))]
public class AddAuthorTests2
{
    private readonly NativeResponseHandler _postAuthorsResponseHandler;

    public AddAuthorTests2()
    {
        _postAuthorsResponseHandler = new NativeResponseHandler();
    }

    [Fact]
    public async Task Create_1_Author()
    {
        await Test.Create_1_Author(_postAuthorsResponseHandler);
    }
}
