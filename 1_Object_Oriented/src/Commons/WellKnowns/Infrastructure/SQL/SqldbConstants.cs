namespace WellKnowns.Infrastructure.SQL;

public static class SqldbConstants
{
    public const int SQLLocalDefaulPort = 50925;
    public const string SQLAdminId = "sa";
    public const string SQLDbName = "alexandria";
    public const string SQLDbLocalSAPassword = "Password1234!";
    public const string SQLDbLocalIP = "127.0.0.1";
    public const string SQLDbMigrationTable = "EFMigrations";
    public const string SQLDbDefaultSchema = "dbo";
    public static string SQLDbLocalConnectionString =>
        $"Server={SQLDbLocalIP},{SQLLocalDefaulPort};User ID={SQLAdminId};Password={SQLDbLocalSAPassword};TrustServerCertificate=true;Database={SQLDbName}";
}
