using Microsoft.Data.SqlClient;
using Respawn;
using WellKnowns.Infrastructure.SQL;

namespace Alexandria.SQLSeeding;

public static class SQLSeeder
{
    public static async Task ResetAsync(
        string sqlConnectionString,
        CancellationToken cancellationToken
    )
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sqlConnectionString, nameof(sqlConnectionString));

        sqlConnectionString = EnforceDatabaseName(sqlConnectionString);
        using var dbConnection = new SqlConnection(sqlConnectionString);

        await dbConnection.OpenAsync(cancellationToken).ConfigureAwait(false);

        var options = new RespawnerOptions()
        {
            WithReseed = true,
            TablesToIgnore = [SqldbConstants.SQLDbMigrationTable],
        };

        var respawner = await Respawner.CreateAsync(dbConnection, options).ConfigureAwait(false);
        await respawner.ResetAsync(sqlConnectionString).ConfigureAwait(false);
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
