using Alexandria.Persistence;
using WellKnowns.Infrastructure.SQL;
using WellKnowns.Presentation.AlexandriaWebApi;

namespace AppHost.Local;

internal static class LocalConfiguration
{
    internal static IDistributedApplicationBuilder Configure(
        this IDistributedApplicationBuilder builder
    )
    {
        // Sql
        var sqldb = ConfigureSqlDb(builder);
        OnDBCreateApplySqlDbMigrations(builder);

        // WebApi
        builder.ConfigureWebApi(sqldb);

        return builder;
    }

    private static IDistributedApplicationBuilder ConfigureWebApi(
        this IDistributedApplicationBuilder builder,
        IResourceBuilder<SqlServerDatabaseResource> sqldb
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
            .WaitFor(sqldb);

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
                SqlProjectReferences.ServerName,
                port: SqldbConstants.SQLLocalDefaulPort,
                password: sqlPassword
            )
            .WithLifetime(ContainerLifetime.Persistent);

        var sqldb = sqlServer.AddDatabase(dbName);
        return sqldb;
    }

    private static void OnDBCreateApplySqlDbMigrations(IDistributedApplicationBuilder builder)
    {
        builder.Eventing.Subscribe<AfterResourcesCreatedEvent>(
            static async (@event, cancellationToken) =>
            {
                var db =
                    @event.Model.Resources.First(x => x.Name == SqlProjectReferences.ServerName)
                    as SqlServerServerResource;

                ArgumentNullException.ThrowIfNull(db, nameof(db));

                var connectionString = await db.GetConnectionStringAsync(cancellationToken)
                    .ConfigureAwait(false);

                ArgumentNullException.ThrowIfNull(connectionString, nameof(connectionString));

                AlexandriaDbContextFactory.ApplyDbMigrations(connectionString);
            }
        );
    }
}
