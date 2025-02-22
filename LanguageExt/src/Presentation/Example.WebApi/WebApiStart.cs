//namespace Example.WebApi;

using Example.WebApi;
using Example.WebApi.Supports.EndpointMapper;

internal static class WebApiStart
{
    internal static WebApplicationBuilder CreateWebAppBuilder(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Host.ConfigureServices((context, services) => services.AddWebApi(context));

        return builder;
    }

    internal static WebApplication CreateWebApp(WebApplicationBuilder builder)
    {
        builder.Services.AddOpenApi();

        var app = builder.Build();

        app.MapGroupedEndpoints();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        return app;
    }
}
