using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Alexandria.Persistence.Audits;

internal abstract class BaseSaveChangesInterceptor<T> : SaveChangesInterceptor
    where T : class
{
    public abstract IEnumerable<EntityEntry<T>> EntitiesToSave(DbContext dbContext);
    public abstract Task OnSaveAsync(
        DbContext dbContext,
        EntityEntry<T> entityEntry,
        CancellationToken cancellationToken
    );

    public sealed override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(eventData, nameof(eventData));
        var dbContext = eventData.Context;
        ArgumentNullException.ThrowIfNull(dbContext, nameof(eventData.Context));
        var entityEntries = EntitiesToSave(dbContext);
        foreach (var entity in entityEntries)
        {
            await OnSaveAsync(dbContext, entity, cancellationToken);
        }

        return await base.SavingChangesAsync(
            eventData,
            result,
            cancellationToken: cancellationToken
        );
    }
}
