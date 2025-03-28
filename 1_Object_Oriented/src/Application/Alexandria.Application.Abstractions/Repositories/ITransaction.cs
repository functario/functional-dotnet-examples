namespace Alexandria.Application.Abstractions.Repositories;

public interface ITransaction : IDisposable
{
    Task BeginAsync(CancellationToken cancellationToken);
    Task CommitAsync(CancellationToken cancellationToken);
    Task RollBackAsync(CancellationToken cancellationToken);
}
