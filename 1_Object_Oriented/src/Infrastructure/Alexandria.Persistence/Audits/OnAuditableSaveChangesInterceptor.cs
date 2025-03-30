using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Alexandria.Persistence.Audits;

internal class OnAuditableSaveChangesInterceptor : BaseSaveChangesInterceptor<IAuditable>
{
    private readonly TimeProvider _timeProvider;

    public OnAuditableSaveChangesInterceptor(TimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
    }

    public override IEnumerable<EntityEntry<IAuditable>> EntitiesToSave(DbContext dbContext) =>
        dbContext.ChangeTracker.Entries<IAuditable>().Where(e => e.State is EntityState.Added);

    public override Task OnSaveAsync(
        DbContext dbContext,
        EntityEntry<IAuditable> entityEntry,
        CancellationToken cancellationToken
    )
    {
        var now = _timeProvider.GetUtcNow();
        Action? setDates = entityEntry.State switch
        {
            EntityState.Added => () =>
            {
                entityEntry.Property("CreatedDate").CurrentValue = now;
                entityEntry.Property("UpdatedDate").CurrentValue = now;
            },
            EntityState.Modified => () => entityEntry.Property("UpdatedDate").CurrentValue = now,
            EntityState.Detached or EntityState.Unchanged => () => { },
            EntityState.Deleted or _ => throw new NotImplementedException(),
        };

        setDates();

        return Task.CompletedTask;
    }
}
