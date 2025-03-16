using WellKnowns.Infrastructure.SQL;
using WellKnowns.Presentation.AlexandriaWebApi;

namespace AppHost.Configurations;

internal static class TestConfiguration
{
    internal static IDistributedApplicationBuilder ConfigureAsTest(
        this IDistributedApplicationBuilder builder
    )
    {
        // Sql
        var (sqlServer, sqldb) = ConfigureSqlDb(builder);

        var migration = builder.ConfigureMigration(sqlServer, sqldb);

        // WebApi
        builder.ConfigureWebApi(sqlServer, sqldb, migration);

        return builder;
    }

    private static IResourceBuilder<ProjectResource> ConfigureMigration(
        this IDistributedApplicationBuilder builder,
        IResourceBuilder<SqlServerServerResource> sqlServer,
        IResourceBuilder<SqlServerDatabaseResource> sqldb
    )
    {
        var migration = builder
            .AddProject<Projects.Alexandria_Migration>("migration")
            .WithEnvironment(SqldbEnvVars.SQLConnectionString, () => SqlConnectionString(sqlServer))
            .WithReference(sqldb)
            .WaitFor(sqldb);

        return migration;
    }

    private static IDistributedApplicationBuilder ConfigureWebApi(
        this IDistributedApplicationBuilder builder,
        IResourceBuilder<SqlServerServerResource> sqlServer,
        IResourceBuilder<SqlServerDatabaseResource> sqldb,
        IResourceBuilder<ProjectResource> migration
    )
    {
        builder
            .AddProject<Projects.Alexandria_WebApi>(WebApiProjectReferences.ProjectName)
            .WithEndpoint()
            .WithEnvironment(SqldbEnvVars.SQLConnectionString, () => SqlConnectionString(sqlServer))
            .WithReference(sqldb)
            .WaitFor(sqldb)
            .WaitForCompletion(migration);

        return builder;
    }

#pragma warning disable CA2012 // Use ValueTasks correctly
    private static string SqlConnectionString(IResourceBuilder<SqlServerServerResource> sqlServer)
    {
        var awaiter = sqlServer.Resource.GetConnectionStringAsync().GetAwaiter();
        while (!awaiter.IsCompleted)
        {
            Thread.Sleep(10);
        }

        var connectionString = awaiter.GetResult();
        return connectionString is null
            ? throw new InvalidOperationException("Could not retrieve SQL ConnectionString")
            : EnforceDatabaseName(connectionString)
                ?? throw new InvalidOperationException("ConnectionString not found");
#pragma warning restore CA2012 // Use ValueTasks correctly
    }

    private static (
        IResourceBuilder<SqlServerServerResource> sqlServer,
        IResourceBuilder<SqlServerDatabaseResource> sqldb
    ) ConfigureSqlDb(IDistributedApplicationBuilder builder)
    {
        var dbName = SqldbConstants.SQLDbName;
        var saPassword = SqldbConstants.SQLDbLocalSAPassword;
        var sqlPassword = builder.AddParameter("sql-password", saPassword, secret: true);

        var sqlServer = builder
            .AddSqlServer(
                SqlProjectReferences.ProjectName,
                port: SqldbConstants.SQLLocalDefaulPort,
                password: sqlPassword
            )
            // If ContainerLifetime.Session, a new Docker container is created for each test
            // but they are only disposed once all tests have run.
            .WithLifetime(ContainerLifetime.Persistent);

        var sqldb = sqlServer.AddDatabase(dbName);
        return (sqlServer, sqldb);
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
