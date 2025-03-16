using WellKnowns.Infrastructure.SQL;
using WellKnowns.Presentation.AlexandriaWebApi;

namespace AppHost.Configurations;

internal static class LocalConfiguration
{
    internal static IDistributedApplicationBuilder ConfigureAsLocal(
        this IDistributedApplicationBuilder builder
    )
    {
        // Sql
        var sqldb = ConfigureSqlDb(builder);

        var migration = builder.ConfigureMigration(sqldb);

        // WebApi
        builder.ConfigureWebApi(sqldb, migration);

        return builder;
    }

    private static IResourceBuilder<ProjectResource> ConfigureMigration(
        this IDistributedApplicationBuilder builder,
        IResourceBuilder<SqlServerDatabaseResource> sqldb
    )
    {
        var migration = builder
            .AddProject<Projects.Alexandria_Migration>("migration")
            .WithEnvironment(
                SqldbEnvVars.SQLConnectionString,
                SqldbConstants.SQLDbLocalConnectionString
            )
            .WithReference(sqldb)
            .WaitFor(sqldb);

        return migration;
    }

    private static IDistributedApplicationBuilder ConfigureWebApi(
        this IDistributedApplicationBuilder builder,
        IResourceBuilder<SqlServerDatabaseResource> sqldb,
        IResourceBuilder<ProjectResource> migration
    )
    {
        builder
            .AddProject<Projects.Alexandria_WebApi>(WebApiProjectReferences.ProjectName)
            .WithEndpoint("https", endpoint => endpoint.IsProxied = false)
            .WithEndpoint("http", endpoint => endpoint.IsProxied = false)
            .WithEnvironment(
                SqldbEnvVars.SQLConnectionString,
                SqldbConstants.SQLDbLocalConnectionString
            )
            .WithReference(sqldb)
            .WaitFor(sqldb)
            .WaitForCompletion(migration);

        return builder;
    }

    private static IResourceBuilder<SqlServerDatabaseResource> ConfigureSqlDb(
        IDistributedApplicationBuilder builder
    )
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
            .WithLifetime(ContainerLifetime.Persistent);

        var sqldb = sqlServer.AddDatabase(dbName);
        return sqldb;
    }
}
