using System.Threading;
using Alexandria.Domain.AuthorDomain;
using Alexandria.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Alexandria.Persistence.Configurations.Interceptors;

internal class Interceptor3 : BaseSaveChangesInterceptor<AuthorModel>
{
    public override IEnumerable<EntityEntry<AuthorModel>> EntitiesToSave(DbContext dbContext) =>
        dbContext.ChangeTracker.Entries<AuthorModel>().Where(e => e.State is EntityState.Added);

    public override Task OnSaveAsync(
        DbContext dbContext,
        AuthorModel book,
        CancellationToken cancellationToken
    )
    {
        return Task.CompletedTask;
    }
}
