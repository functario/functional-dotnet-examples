using Alexandria.Application.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace Alexandria.Persistence;

internal sealed class Transaction : ITransaction
{
    private readonly AlexandriaDbContext _alexandriaDbContext;

    public Guid TransactionId => throw new NotImplementedException();

    public Transaction(AlexandriaDbContext alexandriaDbContext)
    {
        _alexandriaDbContext = alexandriaDbContext;
    }

    private IDbContextTransaction? DbContextTransaction { get; set; }

    public async Task BeginAsync(CancellationToken cancellationToken)
    {
        DbContextTransaction = await _alexandriaDbContext.Database.BeginTransactionAsync(
            cancellationToken
        );
    }

    public Task CommitAsync(CancellationToken cancellationToken)
    {
        return DbContextTransaction is null
            ? throw new InvalidOperationException(
                $"Transaction was not started. Please call '{BeginAsync}'."
            )
            : DbContextTransaction.CommitAsync(cancellationToken);
    }

    public Task RollBackAsync(CancellationToken cancellationToken)
    {
        return DbContextTransaction is null
            ? throw new InvalidOperationException(
                $"Transaction was not started. Please call '{BeginAsync}'."
            )
            : DbContextTransaction.RollbackAsync(cancellationToken);
    }

    public void Dispose()
    {
        _alexandriaDbContext?.Dispose();
    }
}
