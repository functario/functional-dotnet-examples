using Alexandria.Application.Abstractions.Repositories;

namespace Alexandria.Persistence;

internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly AlexandriaDbContext _alexandriaDbContext;

    public UnitOfWork(AlexandriaDbContext alexandriaDbContext)
    {
        _alexandriaDbContext = alexandriaDbContext;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _alexandriaDbContext.SaveChangesAsync(cancellationToken);
    }
}
