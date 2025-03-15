using Alexandria.Persistence;
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
        var (_, sqldb) = ConfigureSqlDb(builder);
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
            .WithEndpoint()
            .WithReference(sqldb)
            .WaitFor(sqldb);

        return builder;
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
            .AddSqlServer(SqlProjectReferences.ProjectName, password: sqlPassword)
            // If ContainerLifetime.Session, a new Docker container is created for each test
            // but they are only disposed once all tests have run.
            .WithLifetime(ContainerLifetime.Session);

        var sqldb = sqlServer.AddDatabase(dbName);
        return (sqlServer, sqldb);
    }

    private static void OnDBCreateApplySqlDbMigrations(IDistributedApplicationBuilder builder)
    {
        builder.Eventing.Subscribe<AfterResourcesCreatedEvent>(
            static async (@event, cancellationToken) =>
            {
                var db =
                    @event.Model.Resources.First(x => x.Name == SqlProjectReferences.ProjectName)
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
