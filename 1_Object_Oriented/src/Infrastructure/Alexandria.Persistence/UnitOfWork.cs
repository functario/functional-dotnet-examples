using Alexandria.Application.Abstractions.Repositories;

namespace Alexandria.Persistence;

internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly AlexandriaDbContext _alexandriaDbContext;

    public UnitOfWork(AlexandriaDbContext alexandriaDbContext)
    {
        _alexandriaDbContext = alexandriaDbContext;
    }

    public async Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken)
    {
        var transaction = new Transaction(_alexandriaDbContext);
        await transaction.BeginAsync(cancellationToken);
        return transaction;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await _alexandriaDbContext.SaveChangesAsync(cancellationToken);
    }
}
