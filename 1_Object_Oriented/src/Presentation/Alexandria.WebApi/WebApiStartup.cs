using Alexandria.WebApi;
using Alexandria.WebApi.Supports.EndpointMapper;
using dotenv.net;
using ServiceDefaults;
using WellKnowns.Presentation.AlexandriaWebApi;

internal static class WebApiStartup
{
    internal static async Task Start(string[] args)
    {
        var builder = WebApiStartup.CreateWebHostBuilder(args);
        var app = await WebApiStartup.BuildWebAppAsync(builder).ConfigureAwait(false);
        await app.RunAsync().ConfigureAwait(false);
    }

    internal static WebApplicationBuilder CreateWebHostBuilder(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        DotEnv.Fluent().WithTrimValues().WithOverwriteExistingVars().Load();

        builder.AddServiceDefaults();

        builder.Host.ConfigureServices((context, services) => services.AddWebApi(context));

        builder.Services.AddOpenApi();
        return builder;
    }

    internal static async Task<WebApplication> BuildWebAppAsync(WebApplicationBuilder builder)
    {
        await Task.Delay(0);

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
