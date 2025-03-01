using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using WellKnowns.Infrastructure.AlexandriaSqldb;

namespace Alexandria.Persistence;

internal class AlexandriaDbContextFactory : IDesignTimeDbContextFactory<AlexandriaDbContext>
{
    public AlexandriaDbContext CreateDbContext(string[] args)
    {
        var sqlConnectionString = args.FirstOrDefault();

        ArgumentNullException.ThrowIfNull(sqlConnectionString, nameof(sqlConnectionString));

        var optionsBuilder = new DbContextOptionsBuilder<AlexandriaDbContext>();
        optionsBuilder =
            (DbContextOptionsBuilder<AlexandriaDbContext>)
                ConfigureDbContextOptionsBuilder(optionsBuilder, sqlConnectionString!);

        return new AlexandriaDbContext(optionsBuilder.Options);
    }

    public static void ApplyDbMigrations(string sqlConnectionString)
    {
        sqlConnectionString = EnforceDatabaseName(sqlConnectionString);
        var factory = new AlexandriaDbContextFactory();
        using var context = factory.CreateDbContext([sqlConnectionString]);
        context.Database.Migrate();
    }

    private static string EnforceDatabaseName(string sqlConnectionString)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(
            sqlConnectionString,
            nameof(sqlConnectionString)
        );

        // Adding the name in the connection string
        // replace the default name "master".
        var databaseSegment = $"Database={Constants.SQLDbName}";
        return sqlConnectionString.Contains(databaseSegment, StringComparison.OrdinalIgnoreCase)
            ? sqlConnectionString
            : $"{sqlConnectionString};{databaseSegment}";
    }

    public static DbContextOptionsBuilder ConfigureDbContextOptionsBuilder(
        DbContextOptionsBuilder optionsBuilder,
        string sqlConnectionString
    )
    {
        optionsBuilder.UseSqlServer(
            sqlConnectionString,
            x =>
                x.MigrationsHistoryTable(
                    Constants.SQLDbMigrationTable,
                    Constants.SQLDbDefaultSchema
                )
        );

        return optionsBuilder;
    }
}
