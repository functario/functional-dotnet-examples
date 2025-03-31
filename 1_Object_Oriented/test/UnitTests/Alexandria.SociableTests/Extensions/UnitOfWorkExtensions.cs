using Alexandria.Application.Abstractions.Repositories;
using NSubstitute;

namespace Alexandria.SociableTests.Extensions;

internal static class UnitOfWorkExtensions
{
    public static IUnitOfWork SetExecuteTransactionAsync<T>(this IUnitOfWork unitOfWork)
    {
        // Configure ExecuteTransactionAsync of the mock to execute the function passed in parameter.
        // So transaction code is called even if UnitOfWork is mocked and no database transaction are commited,
        unitOfWork
            .ExecuteTransactionAsync(
                Arg.Any<Func<IUnitOfWork, CancellationToken, Task<T>>>(),
                Arg.Any<CancellationToken>()
            )
            .ReturnsForAnyArgs(x =>
                x.ArgAt<Func<IUnitOfWork, CancellationToken, Task<T>>>(0)
                    .Invoke(unitOfWork, CancellationToken.None)
            );

        return unitOfWork;
    }
}
