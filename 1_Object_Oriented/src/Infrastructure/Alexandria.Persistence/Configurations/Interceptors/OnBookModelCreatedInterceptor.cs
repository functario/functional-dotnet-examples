using System.Threading;
using Alexandria.Domain.AuthorDomain;
using Alexandria.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Alexandria.Persistence.Configurations.Interceptors;

internal class OnBookModelCreatedInterceptor : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData dbContextEventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(dbContextEventData, nameof(dbContextEventData));
        var dbContext = dbContextEventData.Context;
        ArgumentNullException.ThrowIfNull(dbContext, nameof(dbContextEventData.Context));

        var entries = dbContext
            .ChangeTracker.Entries<BookModel>()
            .Where(e => e.State is EntityState.Added);

        foreach (var entry in entries)
        {
            var book = entry.Entity;
            await MapAuthorsAsync(dbContext, book, cancellationToken);
        }

        return await base.SavingChangesAsync(
            dbContextEventData,
            result,
            cancellationToken: cancellationToken
        );
    }

    private async Task MapAuthorsAsync(
        DbContext dbContext,
        BookModel book,
        CancellationToken cancellationToken
    )
    {
        var missingAuthorIds = new List<long>();
        foreach (var authorId in book.Publication.AuthorsIds)
        {
            var author = await dbContext.FindAsync<AuthorModel>([authorId], cancellationToken);
            if (author is null)
            {
                missingAuthorIds.Add(authorId);
                continue;
            }

            // Join Author and Authors via AuthorsBooks join table.
            book.Authors.Add(author);
        }

        if (missingAuthorIds.Count > 0)
        {
            throw new InvalidOperationException(
                $"The {nameof(Author)}s with following Ids do not exist: [{string.Join(';', missingAuthorIds)}]"
            );
        }
    }
}
