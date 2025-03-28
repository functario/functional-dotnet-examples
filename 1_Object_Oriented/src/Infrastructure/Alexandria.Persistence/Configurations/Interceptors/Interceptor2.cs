using System.Threading;
using Alexandria.Domain.AuthorDomain;
using Alexandria.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Alexandria.Persistence.Configurations.Interceptors;

internal class Interceptor2 : BaseSaveChangesInterceptor<BookModel>
{
    public override IEnumerable<EntityEntry<BookModel>> EntitiesToSave(DbContext dbContext) =>
        dbContext.ChangeTracker.Entries<BookModel>().Where(e => e.State is EntityState.Added);

    public override Task OnSaveAsync(
        DbContext dbContext,
        BookModel book,
        CancellationToken cancellationToken
    )
    {
        return Task.CompletedTask;
    }
}
