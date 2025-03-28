﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
        if (DbContextTransaction is null)
        {
            throw new InvalidOperationException(
                $"Transaction was not started. Please call '{BeginAsync}'."
            );
        }

        return DbContextTransaction.CommitAsync(cancellationToken);
    }

    public Task RollBackAsync(CancellationToken cancellationToken)
    {
        if (DbContextTransaction is null)
        {
            throw new InvalidOperationException(
                $"Transaction was not started. Please call '{BeginAsync}'."
            );
        }

        return DbContextTransaction.RollbackAsync(cancellationToken);
    }

    public void Dispose()
    {
        _alexandriaDbContext?.Dispose();
    }
}
