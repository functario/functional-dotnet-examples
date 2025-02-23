using AlexandriaSQL = WellKnowns.Infrastructure.AlexandriaSqldb.ProjectReferences;
using AlexandriaSQLConstants = WellKnowns.Infrastructure.AlexandriaSqldb.Constants;
using AlexandriaWebApi = WellKnowns.Presentation.AlexandriaWebApi.ProjectReferences;

var builder = DistributedApplication.CreateBuilder(args);

// Database
var dbName = AlexandriaSQLConstants.SQLDbName;
var saPassword = AlexandriaSQLConstants.SQLDbLocalSAPassword;
var dbIP = AlexandriaSQLConstants.SQLDbLocalIP;
var defaultDbPort = AlexandriaSQLConstants.SQLLocalDefaultPort;
var adminId = AlexandriaSQLConstants.SQLAdminId;
var sqlPassword = builder.AddParameter("sql-password", saPassword, secret: true);
var connectionString =
    $"Server={dbIP},{defaultDbPort};User ID={adminId};Password={saPassword};TrustServerCertificate=true;Database={dbName}";

var sqlServer = builder
    .AddSqlServer(
        AlexandriaSQL.ProjectName,
        port: AlexandriaSQLConstants.SQLLocalDefaultPort,
        password: sqlPassword
    )
    .WithLifetime(ContainerLifetime.Persistent);

var sqldb = sqlServer.AddDatabase(dbName);

// WebApi
builder
    .AddProject<Projects.Alexandria_WebApi>(AlexandriaWebApi.ProjectName)
    .WithEndpoint("https", endpoint => endpoint.IsProxied = false)
    .WithEndpoint("http", endpoint => endpoint.IsProxied = false)
    .WithReference(sqldb)
    .WaitFor(sqldb);

builder.Build().Run();
