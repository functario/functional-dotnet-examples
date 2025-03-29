using Alexandria.Persistence.Audits;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Diagnostics;
using WellKnowns.Infrastructure.SQL;

namespace Alexandria.Persistence;

internal class AlexandriaDbContextFactory : IDesignTimeDbContextFactory<AlexandriaDbContext>
{
    public AlexandriaDbContext CreateDbContext(string[] args)
    {
        var sqlConnectionString = args.FirstOrDefault();

        ArgumentNullException.ThrowIfNull(sqlConnectionString, nameof(sqlConnectionString));

        var interceptors = new OnAuditableSavedInterceptor(TimeProvider.System);

        var optionsBuilder = new DbContextOptionsBuilder<AlexandriaDbContext>();
        optionsBuilder =
            (DbContextOptionsBuilder<AlexandriaDbContext>)
                ConfigureDbContextOptionsBuilder(
                    optionsBuilder,
                    sqlConnectionString!,
                    interceptors
                );

        return new AlexandriaDbContext(optionsBuilder.Options);
    }

    public static void ApplyDbMigrations(string sqlConnectionString)
    {
        sqlConnectionString = EnforceDatabaseName(sqlConnectionString);
        var factory = new AlexandriaDbContextFactory();
        using var context = factory.CreateDbContext([sqlConnectionString]);
        context.Database.Migrate();
    }

    public static DbContextOptionsBuilder ConfigureDbContextOptionsBuilder(
        DbContextOptionsBuilder optionsBuilder,
        string sqlConnectionString,
        params IInterceptor[] interceptorps
    )
    {
        // The connection string comming from Aspire could miss the DataBase name depending when it is resolved.
        // if it is missing, the configuration will be on "master" db
        // but later operations will be on "alexandria" once migrations are applied.
        sqlConnectionString = EnforceDatabaseName(sqlConnectionString);

        optionsBuilder
            .UseSqlServer(
                sqlConnectionString,
                x =>
                    x.MigrationsHistoryTable(
                        SqldbConstants.SQLDbMigrationTable,
                        SqldbConstants.SQLDbDefaultSchema
                    )
            )
            .AddInterceptors(interceptorps);

        return optionsBuilder;
    }

    private static string EnforceDatabaseName(string sqlConnectionString)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(
            sqlConnectionString,
            nameof(sqlConnectionString)
        );

        // Adding the name in the connection string
        // replace the default name "master".
        var databaseSegment = $"Database={SqldbConstants.SQLDbName}";
        return sqlConnectionString.Contains(databaseSegment, StringComparison.OrdinalIgnoreCase)
            ? sqlConnectionString
            : $"{sqlConnectionString};{databaseSegment}";
    }
}
