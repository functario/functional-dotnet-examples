namespace WellKnowns.Presentation.ExampleWebApi;

public static class UrlFragments
{
    // https://localhost:7022/openapi/v1.json
    public const string OpenApiVersion = "v1";
    public const string OpenApiContract = $"/openapi/{OpenApiVersion}.json";

    // https://localhost:7022/swagger/index.html
    public const string SwaggerUI = "/swagger/index.html";
}
