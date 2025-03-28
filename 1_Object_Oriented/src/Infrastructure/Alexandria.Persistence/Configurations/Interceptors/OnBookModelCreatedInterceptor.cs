using System.Threading;
using Alexandria.Domain.AuthorDomain;
using Alexandria.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Alexandria.Persistence.Configurations.Interceptors;

internal class OnBookModelCreatedInterceptor : BaseSaveChangesInterceptor<BookModel>
{
    public override IEnumerable<EntityEntry<BookModel>> EntitiesToSave(DbContext dbContext) =>
        dbContext.ChangeTracker.Entries<BookModel>().Where(e => e.State is EntityState.Added);

    public override async Task OnSaveAsync(
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
