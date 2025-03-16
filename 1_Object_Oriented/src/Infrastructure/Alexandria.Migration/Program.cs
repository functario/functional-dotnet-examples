using Alexandria.Persistence;
using WellKnowns.Infrastructure.SQL;

var sqlConnectionString =
    args.ElementAtOrDefault(0)
    ?? Environment.GetEnvironmentVariable(SqldbEnvVars.SQLConnectionString);

ArgumentException.ThrowIfNullOrWhiteSpace(
    sqlConnectionString,
    nameof(SqldbEnvVars.SQLConnectionString)
);

AlexandriaDbContextFactory.ApplyDbMigrations(sqlConnectionString);
