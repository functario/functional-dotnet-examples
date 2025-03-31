namespace Alexandria.Application.Abstractions.Repositories;

public static class UnitOfWorkExtensions
{
    public static async Task<T> ExecuteTransactionAsync<T>(
        this IUnitOfWork unitOfWork,
        Func<IUnitOfWork, CancellationToken, Task<T>> asyncTransaction,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
        ArgumentNullException.ThrowIfNull(asyncTransaction, nameof(asyncTransaction));

        using var transaction = await unitOfWork
            .BeginTransactionAsync(cancellationToken)
            .ConfigureAwait(false);

        try
        {
            var response = await asyncTransaction(unitOfWork, cancellationToken)
                .ConfigureAwait(false);

            await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
            return response;
        }
        catch
        {
            await transaction.RollBackAsync(cancellationToken).ConfigureAwait(false);
            throw;
        }
    }
}
