namespace Alexandria.Application.Abstractions.Repositories;

public interface IUnitOfWork
{
    Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken);

    Task<T> ExecuteTransactionAsync<T>(
        Func<IUnitOfWork, CancellationToken, Task<T>> operationAsync,
        CancellationToken cancellationToken
    );

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
