//namespace Example.WebApi;

using Example.WebApi;
using Example.WebApi.Supports.EndpointMapper;
using ServiceDefaults;
using WellKnowns.Presentation.ExampleWebApi;

internal static class WebApiStart
{
    internal static WebApplicationBuilder CreateWebAppBuilder(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.AddServiceDefaults();

        builder.Host.ConfigureServices((context, services) => services.AddWebApi(context));

        builder.Services.AddOpenApi();
        return builder;
    }

    internal static WebApplication CreateWebApp(WebApplicationBuilder builder)
    {
        var app = builder.Build();

        app.MapGroupedEndpoints();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapDefaultEndpoints();
            app.MapOpenApi();
            app.UseSwaggerUI(x =>
                x.SwaggerEndpoint(UrlFragments.OpenApiContract, UrlFragments.OpenApiVersion)
            );
        }

        app.UseHttpsRedirection();
        return app;
    }
}
