using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Alexandria.Persistence.Audits;

internal class OnAuditableSavedInterceptor : BaseSaveChangesInterceptor<IAuditable>
{
    private readonly TimeProvider _timeProvider;

    public OnAuditableSavedInterceptor(TimeProvider timeProvider)
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
        entityEntry.Property("CreatedDate").CurrentValue = now;
        entityEntry.Property("UpdatedDate").CurrentValue = now;
        return Task.CompletedTask;
    }
}
