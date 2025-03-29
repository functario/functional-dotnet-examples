using Alexandria.Application.AuthorUseCases.AddAuthor;
using Alexandria.Application.AuthorUseCases.GetAuthor;
using Alexandria.Application.BookUseCases.AddBook;
using Alexandria.Application.BookUseCases.GetBook;
using Alexandria.Application.BookUseCases.GetBookAuthors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Alexandria.Application;

public static class ServiceCollectionsExtensions
{
    public static IServiceCollection AddAlexandriaApplication(
        this IServiceCollection services,
        HostBuilderContext _
    )
    {
        return services.WithApplicationServices();
    }

    internal static IServiceCollection WithApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAddAuthorService, AddAuthorService>();
        services.AddScoped<IGetAuthorService, GetAuthorService>();
        services.AddScoped<IAddBookService, AddBookService>();
        services.AddScoped<IGetBookService, GetBookService>();
        services.AddScoped<IGetBookAuthorsService, GetBookAuthorsService>();

        return services;
    }
}
